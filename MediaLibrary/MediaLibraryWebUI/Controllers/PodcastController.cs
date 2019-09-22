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
        private readonly PodcastViewModel podcastViewModel;

        [ImportingConstructor]
        public PodcastController(IPodcastService podcastService, IDataService dataService, PodcastViewModel podcastViewModel)
        {
            this.podcastService = podcastService;
            this.dataService = dataService;
            this.podcastViewModel = podcastViewModel;
        }

        public async Task<ActionResult> Index()
        {
            podcastViewModel.Podcasts = await dataService.GetList<Podcast>();

            return View(podcastViewModel);
        }
    }
}