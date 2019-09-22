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
    [Export("Music", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class MusicController : Controller
    {
        private readonly IDataService dataService;
        private readonly IMusicService musicService;

        [ImportingConstructor]
        public MusicController(IDataService dataService, IMusicService musicService)
        {
            this.dataService = dataService;
            this.musicService = musicService;
    }
        
        public async Task<ActionResult> Index()
        {
            MusicViewModel model = new MusicViewModel();

            model.Songs = await musicService.GetSongGroups();
            model.Artists = await musicService.GetArtistGroups();
            model.Albums = await musicService.GetAlbumGroups();

            return View(model);
        }
    }
}