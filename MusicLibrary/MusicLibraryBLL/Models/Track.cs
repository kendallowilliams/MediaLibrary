using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TagLib;

namespace MusicLibraryBLL.Models
{
    public class Track : BaseModel
    {
        public Track() { }

        public Track(MediaData data, int? pathId, int? genreId, int? albumId, int? artistId, int? fileId = null)
        {
            Title = data.Title;
            FileName = data.FileName;
            PathId = pathId;
            FileId = fileId;
            AlbumId = albumId;
            GenreId = genreId;
            ArtistId = artistId;
            Position = (int)data.Track;
            Year = (int)data.Year;
            Duration = data.Duration;
            PlayCount = 0;
        }

        public string Title { get; set; }
        public string FileName { get; set; }
        public int? PathId { get; set; }
        public int? FileId { get; set; }
        public int? AlbumId { get; set; }
        public int? GenreId { get; set; }
        public int? ArtistId { get; set; }
        public int Position { get; set; }
        public int Year { get; set; }
        public double Duration { get; set; }
        public string DurationDisplay => TimeSpan.FromSeconds(Duration).ToString(@"m\:ss");
        public int PlayCount { get; set; }
    }
}