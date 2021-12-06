﻿using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class SpotifyContext : DbContext
    {
        public SpotifyContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<TrackModel> Track { get; set; }
    }
}
