using Data.Models;
using Data.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest.Api.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    //[Route("api")]
    public class TrackController : Controller
    {
        ITrackService _controllerService;
        public TrackController(ITrackService service)
        {
            _controllerService = service;
        }

        /// <summary>
        /// get all tracks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("[action]")]
        public async Task<IActionResult> GetAllTracks()
        {
            try
            {
                var tracks = await _controllerService.GetTracksList();
                if (tracks == null) return NotFound();
                return Ok(tracks);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// get track details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTrackById(int id)
        {
            try
            {
                var track = await _controllerService.GetTrackDetailsById(id);
                if (track == null) return NotFound();
                return Ok(track);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// save track
        /// </summary>
        /// <param name="trackModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("[action]")]
        public async Task<IActionResult> SaveEmployees(TrackModel trackModel)
        {
            try
            {
                var model = await _controllerService.SaveTrack(trackModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// delete track
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        //[Route("[action]")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try
            {
                var model = await _controllerService.DeleteTrack(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
