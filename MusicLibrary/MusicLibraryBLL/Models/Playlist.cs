using MusicLibraryBLL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicLibraryBLL.Models
{
    public class Playlist : BaseModel
    {
        public Playlist()
        {
            TrackIds = Enumerable.Empty<int>();
        }

        public Playlist(string name, IEnumerable<int> trackIds = null)
        {
            Name = name;
            TrackIds = trackIds ?? Enumerable.Empty<int>();
        }

        public string Name { get; set; }
        public IEnumerable<int> TrackIds { get; set; }
    }
}