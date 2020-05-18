using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaLibraryDAL.Partials.Interfaces
{
    public interface IEpisodeJSON
    {
        ICollection<PlaylistEpisode> PlaylistEpisodes { get; set; }
    }
}
