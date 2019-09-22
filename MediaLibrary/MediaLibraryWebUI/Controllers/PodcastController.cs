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
    [Export("Podcast", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : Controller
    {
        private readonly IPodcastService podcastService;

        [ImportingConstructor]
        public PodcastController(IPodcastService podcastService)
        {
            this.podcastService = podcastService;
        }

        public async Task<ActionResult> Index()
        {
            PodcastViewModel model = new PodcastViewModel();

            model.Podcasts = await podcastService.GetPodcastGroups();

            return View(model);
        }
    }
}