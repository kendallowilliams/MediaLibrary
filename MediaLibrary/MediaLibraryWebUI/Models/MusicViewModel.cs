using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public class MusicViewModel
    {
        private IEnumerable<Track> songs;
        private IEnumerable<Artist> artists;
        private IEnumerable<Album> albums;

        public MusicViewModel()
        {
            songs = Enumerable.Empty<Track>();
            artists = Enumerable.Empty<Artist>();
            albums = Enumerable.Empty<Album>();
        }

        public IEnumerable<Track> Songs { get => songs; set => songs = value; }
        public IEnumerable<Artist> Artists { get => artists; set => artists = value; }
        public IEnumerable<Album> Albums { get => albums; set => albums = value; }
    }
}