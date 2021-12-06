using Data.Models;
using SpotifyApi.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ISpotifyService
    {
        /// <summary>
        /// get track details by ISRC from the spotify API
        /// </summary>
        /// <param name="isrc"></param>
        /// <returns></returns>
        Task<TrackModel> GetTrackDetailsByISRCAsync(string isrc);

        /// <summary>
        /// get track details by title from the spotify API
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<List<TrackModel>> GetTrackDetailsByTitle(string title);
    }
}
