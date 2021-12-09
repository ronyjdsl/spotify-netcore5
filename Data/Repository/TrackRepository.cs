using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class TrackRepository : GenericRepository<TrackModel>, ITrackRepository
    {
        public TrackRepository(SpotifyContext context, ILogger logger) : base(context, logger) { }

        public override async Task<IEnumerable<TrackModel>> All()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} All function error", typeof(TrackRepository));
                return new List<TrackModel>();
            }
        }

        public override async Task<bool> Upsert(TrackModel entity)
        {
            try
            {
                var existingRecord = await dbSet.Where(x => x.Id == entity.Id)
                                                    .FirstOrDefaultAsync();

                if (existingRecord == null)
                    return await Add(entity);

                existingRecord.duration_ms = entity.duration_ms;
                existingRecord.ISRC = entity.ISRC;
                existingRecord.is_explicit = entity.is_explicit;
                existingRecord.Name = entity.Name;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert function error", typeof(TrackRepository));
                return false;
            }
        }

        public override async Task<bool> Delete(int id)
        {
            try
            {
                var exist = await dbSet.Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();

                if (exist == null) return false;

                dbSet.Remove(exist);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete function error", typeof(TrackRepository));
                return false;
            }
        }
    }
}
