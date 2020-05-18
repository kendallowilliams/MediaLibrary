using MediaLibraryDAL.DbContexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaLibraryDAL.Partials.Interfaces
{
    public interface IPlaylistPodcastItemJSON
    {
        [JsonIgnore]
        Playlist Playlist { get; set; }
    }
}
