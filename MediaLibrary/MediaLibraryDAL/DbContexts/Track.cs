using System;
using System.Collections.Generic;

namespace MediaLibraryDAL.DbContexts
{
    public partial class Track
    {
        public Track()
        {
            PlaylistTracks = new HashSet<PlaylistTrack>();
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public int? PathId { get; set; }
        public string Title { get; set; }
        public int? AlbumId { get; set; }
        public int? GenreId { get; set; }
        public int? ArtistId { get; set; }
        public int? Position { get; set; }
        public int? Year { get; set; }
        public decimal Duration { get; set; }
        public int Progress { get; set; }
        public int PlayCount { get; set; }
        public DateTime? LastPlayedDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public virtual Album Album { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual TrackPath Path { get; set; }
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
