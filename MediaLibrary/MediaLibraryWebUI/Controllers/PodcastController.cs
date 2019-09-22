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
    [Export("Podcast", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : Controller
    {
        private readonly IPodcastService podcastService;
        private readonly IDataService dataService;

        [ImportingConstructor]
        public PodcastController(IPodcastService podcastService, IDataService dataService)
        {
            this.podcastService = podcastService;
            this.dataService = dataService;
        }

        public async Task<ActionResult> Index()
        {
            PodcastViewModel model = new PodcastViewModel();

            model.Podcasts = await dataService.GetList<Podcast>();

            return View(model);
        }
    }
}