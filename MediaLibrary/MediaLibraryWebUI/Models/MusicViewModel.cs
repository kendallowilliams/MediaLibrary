using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public class MusicViewModel
    {
        private IEnumerable<Track> tracks;

        public MusicViewModel() { }

        public IEnumerable<Track> Tracks { get => tracks; set => tracks = value; }
    }
}