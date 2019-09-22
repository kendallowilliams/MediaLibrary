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

namespace MediaLibraryWebUI.Controllers
{
    [Export("Playlist", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService playlistService;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PlaylistController(IPlaylistService playlistService, IDataService dataService)
        {
            this.playlistService = playlistService;
            this.dataService = dataService;
        }

        public async Task<ActionResult> Index()
        {
            PlaylistViewModel model = new PlaylistViewModel();

            model.Playlists = await dataService.GetList<Playlist>();

            return View(model);
        }
    }
}