using MediaLibraryDAL.Models;
using MediaLibraryDAL.Models.Interfaces;
using MediaLibraryDAL.Partials.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryDAL.DbContexts
{
    public partial class PlaylistEpisode : IDataModel, IPlaylistEpisodeJSON
    {
        public PlaylistEpisode() { }
    }
}
