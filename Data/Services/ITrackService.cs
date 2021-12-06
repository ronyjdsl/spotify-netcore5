using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public interface ITrackService
    {
        /// <summary>
        /// get list of all Tracks
        /// </summary>
        /// <returns></returns>
        Task<List<TrackModel>> GetTracksList();

        /// <summary>
        /// get track details by track id
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        Task<TrackModel> GetTrackDetailsById(int trackId);

        /// <summary>
        /// get track details by track id
        /// </summary>
        /// <param name="isrc"></param>
        /// <returns></returns>
        Task<TrackModel> GetTrackDetailsByISRC(string isrc);

        /// <summary>
        ///  add edit track
        /// </summary>
        /// <param name="trackModel"></param>
        /// <returns></returns>
        Task<ResponseModel> SaveTrack(TrackModel trackModel);


        /// <summary>
        /// delete Tracks
        /// </summary>
        /// <param name="trackId"></param>
        /// <returns></returns>
        Task<ResponseModel> DeleteTrack(int trackId);
    }
}
