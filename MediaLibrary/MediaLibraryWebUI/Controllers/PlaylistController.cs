using MediaLibraryWebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Playlist", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaylistController : Controller
    {
        [ImportingConstructor]
        public PlaylistController() { }

        public ActionResult Index()
        {
            PlaylistViewModel model = new PlaylistViewModel();

            return View(model);
        }
    }
}