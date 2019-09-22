using MediaLibraryWebUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
    [Export("Podcast", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : Controller
    {
        [ImportingConstructor]
        public PodcastController() { }

        public ActionResult Index()
        {
            PodcastViewModel model = new PodcastViewModel();

            return View(model);
        }
    }
}