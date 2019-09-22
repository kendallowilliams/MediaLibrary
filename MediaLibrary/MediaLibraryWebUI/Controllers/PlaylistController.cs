using MediaLibraryWebUI.Models;
using MediaLibraryWebUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Playlist", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService playlistService;

        [ImportingConstructor]
        public PlaylistController(IPlaylistService playlistService)
        {
            this.playlistService = playlistService;
        }

        public async Task<ActionResult> Index()
        {
            PlaylistViewModel model = new PlaylistViewModel();

            model.Playlists = await playlistService.GetPlaylistGroups();

            return View(model);
        }
    }
}