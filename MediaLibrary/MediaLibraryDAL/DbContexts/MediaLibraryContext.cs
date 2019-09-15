using MediaLibraryDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryDAL.DbContexts
{
    public class MediaLibraryContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<PodcastFile> PodcastFiles { get; set; }
        public DbSet<PodcastItem> PodcastItems { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackFile> TrackFiles { get; set; }
        public DbSet<TrackPath> TrackPaths { get; set; }
        public DbSet<Transaction> Tranactions { get; set; }

        public MediaLibraryContext() : base() { }
    }
}
