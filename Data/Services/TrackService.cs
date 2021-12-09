using Data.Context;
using Data.Models;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TrackService : ITrackService
    {
        private readonly ILogger<TrackService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public TrackService(
            ILogger<TrackService> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }


        public async Task<ResponseModel> DeleteTrack(int trackId)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                TrackModel _temp = await GetTrackDetailsById(trackId);
                if (_temp != null)
                {
                    await _unitOfWork.Tracks.Delete(trackId);
                    model.IsSuccess = true;
                    model.Messsage = "Track Deleted Successfully";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Track Not Found";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }

        public async Task<TrackModel> GetTrackDetailsById(int trackId)
        {
            TrackModel track;
            try
            {
                track = await _unitOfWork.Tracks.GetById(trackId);
            }
            catch (Exception)
            {
                throw;
            }
            return track;
        }

        public async Task<TrackModel> GetTrackDetailsByISRC(string isrc)
        {
            try
            {
                var data = await _unitOfWork.Tracks.Get(p => p.ISRC == isrc);
                if (data != null && data.Count() > 0)
                {
                    return data.First();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<List<TrackModel>> GetTracksList()
        {
            List<TrackModel> trackList;
            try
            {
                trackList = (List<TrackModel>)await _unitOfWork.Tracks.All();
            }
            catch (Exception)
            {
                throw;
            }
            return trackList;
        }

        public async Task<ResponseModel> SaveTrack(TrackModel trackModel)
        {
            ResponseModel model = new ResponseModel();
            if (trackModel == null)
            {
                model.IsSuccess = false;
                model.Messsage = "Unable to save track in the DB, the details were not found";
                return model;
            }

            try
            {
                await _unitOfWork.Tracks.Upsert(trackModel);
                //TrackModel _temp = await GetTrackDetailsById(trackModel.Id);
                //if (_temp != null)
                //{
                //    _temp.ISRC = trackModel.ISRC;
                //    _temp.Name = trackModel.Name;
                //    _temp.duration_ms = trackModel.duration_ms;
                //    _temp.is_explicit = trackModel.is_explicit;
                //    _context.Update<TrackModel>(_temp);
                //    model.Messsage = "Track Update Successfully";
                //}
                //else
                //{
                //    _context.Add<TrackModel>(trackModel);
                //    model.Messsage = "Track Inserted Successfully";
                //}
                //_context.SaveChanges();
                model.Messsage = "Track Saved Successfully";
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error : " + ex.Message;
            }
            return model;
        }
    }
}
