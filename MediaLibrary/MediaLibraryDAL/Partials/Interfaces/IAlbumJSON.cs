using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLibraryDAL.DbContexts;
using Newtonsoft.Json;

namespace MediaLibraryDAL.Partials.Interfaces
{
    public interface IAlbumJSON
    {
        [JsonIgnore]
        Artist Artist { get; set; }
        [JsonIgnore]
        Genre Genre { get; set; }
        [JsonIgnore]
        ICollection<Track> Track { get; set; }
    }
}
