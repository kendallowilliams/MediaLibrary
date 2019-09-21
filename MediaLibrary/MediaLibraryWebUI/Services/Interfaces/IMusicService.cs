using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Services.Interfaces
{
    public interface IMusicService
    {
        IEnumerable<IGrouping<string, Track>> GetSongGroups(IEnumerable<Track> songs, SongSort sort);
    }
}
