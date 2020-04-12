using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MediaLibraryWebUI.UIEnums;
using Fody;
using MediaLibraryWebUI.Models;

namespace MediaLibraryWebUI.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IPlaylistUIService))]
    public class PlaylistUIService : BaseUIService, IPlaylistUIService
    {
        private readonly Lazy<IDataService> lazyDataService;
        private IDataService dataService => lazyDataService.Value;

        [ImportingConstructor]
        public PlaylistUIService(Lazy<IDataService> dataService) : base()
        {
            this.lazyDataService = dataService;
        }

        public async Task<IEnumerable<IGrouping<string, Playlist>>> GetPlaylistGroups(PlaylistSort sort)
        {
            IEnumerable<IGrouping<string, Playlist>> groups = null;
            IEnumerable<Playlist> playlists = await dataService.GetList<Playlist>(default, default, playlist => playlist.PlaylistTracks.Select(item => item.Track));
           
            switch (sort)
            {
                case PlaylistSort.DateAdded:
                    groups = playlists.GroupBy(playlist => playlist.ModifyDate.ToString("MM-dd-yyyy")).OrderByDescending(group => DateTime.Parse(group.Key));
                    break;
                case PlaylistSort.AtoZ:
                default:
                    groups = GetPlaylistsAtoZ(playlists.OrderBy(playlist => playlist.Name));
                    break;
            }

            return groups;
        }

        private IEnumerable<IGrouping<string, Playlist>> GetPlaylistsAtoZ(IEnumerable<Playlist> playlists)
        {
            return playlists.GroupBy(playlist => getCharLabel(playlist.Name)).OrderBy(group => group.Key);
        }
    }
}