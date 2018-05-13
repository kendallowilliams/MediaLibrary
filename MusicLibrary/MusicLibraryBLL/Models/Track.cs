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

        public Track(Tag tag, string fileName, int pathId, int albumId, int genreId, int artistId, int duration)
        {
            Title = tag.Title;
            FileName = fileName;
            PathId = pathId;
            AlbumId = albumId;
            GenreId = genreId;
            ArtistId = artistId;
            Number = tag.Track;
            Year = tag.Year;
            Duration = duration;
            PlayCount = 0;
        }

        public string Title { get; set; }
        public string FileName { get; set; }
        public int PathId { get; set; }
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public uint Number { get; set; }
        public uint Year { get; set; }
        public int Duration { get; set; }
        public int PlayCount { get; set; }
    }
}