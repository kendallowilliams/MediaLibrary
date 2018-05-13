using MusicLibraryWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryWebApi.Models
{
    public class Album : BaseModel
    {
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int GenreId { get; set; }
        public string Year { get; set; }
    }
}