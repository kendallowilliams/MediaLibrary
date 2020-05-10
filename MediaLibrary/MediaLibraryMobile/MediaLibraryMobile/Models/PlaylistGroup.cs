using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaLibraryMobile.Models
{
    public class PlaylistGroup : List<Playlist>
    {
        public PlaylistGroup() : base(Enumerable.Empty<Playlist>())
        {
            Name = string.Empty;
        }

        public PlaylistGroup(string name, IEnumerable<Playlist> playlists) : base(playlists)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
