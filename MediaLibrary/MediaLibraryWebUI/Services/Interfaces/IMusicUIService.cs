using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Services.Interfaces
{
    public interface IMusicUIService
    {
        Task<IEnumerable<Track>> Songs();
        Task<IEnumerable<Artist>> Artists();
        Task<IEnumerable<Album>> Albums();
        Task<IEnumerable<IGrouping<string, Track>>> GetSongGroups(SongSort sort = default(SongSort));
        Task<IEnumerable<IGrouping<string, Album>>> GetAlbumGroups(AlbumSort sort = default(AlbumSort));
        Task<IEnumerable<IGrouping<string, Artist>>> GetArtistGroups(ArtistSort sort = default(ArtistSort));
        void ClearData();
    }
}
