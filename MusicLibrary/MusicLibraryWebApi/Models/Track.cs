using MusicLibraryWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryWebApi.Models
{
    public class Track : BaseModel
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public int PathId { get; set; }
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int Number { get; set; }
        public string Year { get; set; }
        public int Length { get; set; }
        public int PlayCount { get; set; }
    }
}