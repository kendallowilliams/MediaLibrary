using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaLibraryDAL.Models;

namespace MediaLibraryWebUI.Models
{
    public class MusicViewModel
    {
        private IEnumerable<Track> tracks;

        public MusicViewModel()
        {
            tracks = Enumerable.Empty<Track>();
        }

        public IEnumerable<Track> Tracks { get => tracks; set => tracks = value; }
    }
}
