using MediaLibraryBLL.Services.Interfaces;
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
    [Export("Podcast", typeof(IController)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class PodcastController : Controller
    {
        private readonly IPodcastUIService podcastUIService;
        private readonly IDataService dataService;
        private readonly PodcastViewModel podcastViewModel;
        private readonly IPodcastService podcastService;

        [ImportingConstructor]
        public PodcastController(IPodcastUIService podcastUIService, IDataService dataService, PodcastViewModel podcastViewModel,
                                 IPodcastService podcastService)
        {
            this.podcastUIService = podcastUIService;
            this.dataService = dataService;
            this.podcastViewModel = podcastViewModel;
            this.podcastService = podcastService;
        }

        public async Task<ActionResult> Index()
        {
            return await Sort(PodcastSort.AtoZ);
        }

        public async Task<ActionResult> Sort(PodcastSort sort)
        {
            podcastViewModel.PodcastGroups = await podcastUIService.GetPodcastGroups(sort);

            return View("Index", podcastViewModel);
        }

        public async Task<ActionResult> AddPodcast(string rssFeed)
        {
            Podcast podcast = await podcastService.AddPodcast(rssFeed);
            
            podcastViewModel.SelectedPodcast = podcast;

            return View("Podcast", podcastViewModel);
        }

        public async Task<ActionResult> RemovePodcast(int id)
        {
            await dataService.DeleteAll<PodcastItem>(item => item.PodcastId == id);
            await dataService.Delete<Podcast>(id);

            return await Index();
        }

        public async Task<ActionResult> Get(int id)
        {
            podcastViewModel.SelectedPodcast = await dataService.GetAsync<Podcast, ICollection<PodcastItem>>(podcast => podcast.Id == id, podcast => podcast.PodcastItems);

            return View("Podcast", podcastViewModel);
        }

        public async Task<ActionResult> DownloadPodcastItem(int id)
        {
            await podcastService.AddPodcastFile(null, id);

            return View("Podcast", podcastViewModel);
        }

        public async Task<ActionResult> RefreshPodcast(int id)
        {
            Podcast podcast = await dataService.GetAsync<Podcast>(item => item.Id == id);

            await podcastService.UpdatePodcast(podcast);
            podcast = await dataService.GetAsync<Podcast>(item => item.Id == id);

            return View("Podcast", podcastViewModel);
        }
    }
}