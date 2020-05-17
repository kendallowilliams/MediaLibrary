using MediaLibraryDAL.DbContexts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryDAL.Partials.Interfaces
{
    public interface IPodcast
    {
        [JsonIgnore]
        ICollection<PodcastItem> PodcastItems { get; set; }
    }
}
