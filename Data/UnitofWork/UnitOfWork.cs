using Data.Context;
using Data.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitofWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly SpotifyContext _context;
        private readonly ILogger _logger;

        public ITrackRepository Tracks { get; private set; }

        public UnitOfWork(SpotifyContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

            Tracks = new TrackRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
