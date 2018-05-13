using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Album : BaseModel
    {
        public Album() { }

        public Album(string title, int artistId, int genreId, uint year)
        {
            Title = title;
            ArtistId = artistId;
            GenreId = genreId;
            Year = year;
        }

        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        public uint Year { get; set; }
    }
}