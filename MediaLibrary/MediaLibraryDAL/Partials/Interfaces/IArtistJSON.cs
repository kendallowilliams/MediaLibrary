using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaLibraryDAL.DbContexts;
using Newtonsoft.Json;

namespace MediaLibraryDAL.Partials.Interfaces
{
    public interface IArtistJSON
    {
        [JsonIgnore]
        ICollection<Album> Album { get; set; }
        [JsonIgnore]
        ICollection<Track> Track { get; set; }
    }
}
