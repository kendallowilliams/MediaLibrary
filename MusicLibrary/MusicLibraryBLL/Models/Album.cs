using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Album : BaseModel
    {
        public Album() { }

        public Album(int id, string title)
        {
            Id = id;
            Title = title;
            ArtistId = -1;
            GenreId = -1;
        }

        public Album(MediaData data, int? artistId, int? genreId)
        {
            Title = data.Album;
            ArtistId = artistId;
            GenreId = genreId;
            Year = (int)data.Year;
        }

        public string Title { get; set; }
        public int? ArtistId { get; set; }
        public int? GenreId { get; set; }
        public int Year { get; set; }
    }
}