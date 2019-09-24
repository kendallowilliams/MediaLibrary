using MediaLibraryDAL.DbContexts;
using MediaLibraryDAL.Services.Interfaces;
using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Playlist", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistUIService playlistService;
        private readonly IDataService dataService;
        private readonly PlaylistViewModel playlistViewModel;

        [ImportingConstructor]
        public PlaylistController(IPlaylistUIService playlistService, IDataService dataService, PlaylistViewModel playlistViewModel)
        {
            this.playlistService = playlistService;
            this.dataService = dataService;
            this.playlistViewModel = playlistViewModel;
        }

        public async Task<ActionResult> Index()
        {
            return await Sort(PlaylistSort.AtoZ);
        }

        public async Task<ActionResult> Sort(PlaylistSort sort)
        {
            playlistViewModel.PlaylistGroups = await playlistService.GetPlaylistGroups(sort);

            return View("Index", playlistViewModel);
        }

        public async Task<ActionResult> AddPlaylist(string playlistName)
        {
            Playlist playlist = new Playlist(playlistName);

            await dataService.Insert(playlist);
            playlistViewModel.SelectedPlaylist = playlist;

            return View("Playlist", playlistViewModel);
        }

        public async Task<ActionResult> RemovePlaylist(int id)
        {
            await dataService.DeleteAll<PlaylistTrack>(track => track.PlaylistId == id);
            await dataService.Delete<Playlist>(id);

            return await Index();
        }

        public async Task<ActionResult> Get(int id)
        {
            playlistViewModel.SelectedPlaylist = await dataService.GetAsync<Playlist, IEnumerable<Track>>(item => item.Id == id, 
                                                                                                          playlist => playlist.PlaylistTracks.Select(list => list.Track));

            return View("Playlist", playlistViewModel);
        }

        public async Task<ActionResult> RemovePlaylistItem(int id, int playlistId)
        {
            await dataService.Delete<PlaylistTrack>(id);
            playlistViewModel.SelectedPlaylist = await dataService.GetAsync<Playlist, IEnumerable<Track>>(item => item.Id == playlistId,
                                                                                                          playlist => playlist.PlaylistTracks.Select(list => list.Track));

            return View("Playlist", playlistViewModel);
        }
    }
}