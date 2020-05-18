using MediaLibraryDAL.DbContexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaLibraryDAL.Partials.Interfaces
{
    public interface IPlaylistTrackJSON
    {
        [JsonIgnore]
        Playlist Playlist { get; set; }
    }
}
