using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PodcastViewModel : ViewModel
    {
        private IEnumerable<IGrouping<string, Podcast>> podcastGroups;
        private readonly HomeViewModel homeViewModel;
        private Podcast selectedPodcast;
        private IEnumerable<PodcastItem> podcastItems;
        private PodcastConfiguration configuration;

        [ImportingConstructor]
        public PodcastViewModel(HomeViewModel homeViewModel)
        {
            podcastGroups = Enumerable.Empty<IGrouping<string, Podcast>>();
            this.homeViewModel = homeViewModel;
            podcastItems = Enumerable.Empty<PodcastItem>();
            configuration = new PodcastConfiguration();
        }

        public IEnumerable<IGrouping<string, Podcast>> PodcastGroups { get => podcastGroups; set => podcastGroups = value; }
        public Podcast SelectedPodcast { get => selectedPodcast; set => selectedPodcast = value; }
        public IEnumerable<PodcastItem> PodcastItems { get => podcastItems; set => podcastItems = value; }
        public HomeViewModel HomeViewModel { get => homeViewModel; }
        public PodcastConfiguration Configuration { get => configuration; set => configuration = value ?? new PodcastConfiguration(); }
    }
}