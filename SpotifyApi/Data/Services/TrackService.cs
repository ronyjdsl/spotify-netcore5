using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class TrackService : ITrackService
    {
        private SpotifyContext _context;
        public TrackService(SpotifyContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel> DeleteTrack(int trackId)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                TrackModel _temp = await GetTrackDetailsById(trackId);
                if (_temp != null)
                {
                    _context.Remove<TrackModel>(_temp);
                    _context.SaveChanges();
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
                track = await _context.FindAsync<TrackModel>(trackId);
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
                var data = await _context.Track.Where(obj => obj.ISRC == isrc).ToListAsync();
                if (data != null && data.Count() > 0)
                {
                    return data[0];
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
                trackList = await _context.Set<TrackModel>().ToListAsync();
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
                TrackModel _temp = await GetTrackDetailsById(trackModel.Id);
                if (_temp != null)
                {
                    _temp.ISRC = trackModel.ISRC;
                    _temp.Name = trackModel.Name;
                    _temp.duration_ms = trackModel.duration_ms;
                    _temp.is_explicit = trackModel.is_explicit;
                    _context.Update<TrackModel>(_temp);
                    model.Messsage = "Track Update Successfully";
                }
                else
                {
                    _context.Add<TrackModel>(trackModel);
                    model.Messsage = "Track Inserted Successfully";
                }
                _context.SaveChanges();
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
