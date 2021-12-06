using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CodeChallengeController : Controller
    {
        ITrackService _trackService;
        ISpotifyService _spotifyService;
        public CodeChallengeController(ITrackService theTrackService, ISpotifyService theSpotifyService)
        {
            _trackService = theTrackService;
            _spotifyService = theSpotifyService;
        }

        /// <summary>
        /// get track details by isrc
        /// </summary>
        /// <param name="isrc"></param>
        /// <returns></returns>
        [HttpGet]
        // https://localhost:44367/api/CodeChallenge/GetTrack?isrc=abc123
        public async Task<IActionResult> GetTrack(string isrc)
        {
            try
            {
                var track = await _trackService.GetTrackDetailsByISRC(isrc);
                if (track == null) return NotFound();
                return Ok(track);
            }
            catch (Exception)
            {
                return StatusCode(500, new ResponseModel() { IsSuccess = false, Messsage = "Something went wrong, please retry later" });
            }
        }

        /// <summary>
        /// creates a new track
        /// </summary>
        /// <param name="isrc"></param>
        /// <returns></returns>
        [HttpPost]
        // https://localhost:44367/api/CodeChallenge/CreateTrack?isrc=abc123
        public async Task<IActionResult> CreateTrack(string isrc)
        {
            try
            {
                var trackExists = await _trackService.GetTrackDetailsByISRC(isrc);
                if (trackExists != null) return StatusCode(400, new ResponseModel() { IsSuccess = false, Messsage = "ISRC already exists" });

                TrackModel track = await _spotifyService.GetTrackDetailsByISRCAsync(isrc);
                if (track == null)
                {
                    return StatusCode(400, new ResponseModel() { IsSuccess = false, Messsage = "Unable to find ISRC" });
                }

                var resp = await _trackService.SaveTrack(track);
                if (resp.IsSuccess)
                {
                    // return Ok(track);
                    return Ok(resp);
                }
                return StatusCode(500, resp);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// get track details by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        // https://localhost:44367/api/CodeChallenge/GetTrackDetailsByTitle?title=Livin' on a Prayer
        public async Task<IActionResult> GetTrackDetailsByTitle(string title)
        {
            try
            {
                var trackDetails = await _spotifyService.GetTrackDetailsByTitle(title);
                if (trackDetails == null) return NotFound();
                return Ok(trackDetails);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
