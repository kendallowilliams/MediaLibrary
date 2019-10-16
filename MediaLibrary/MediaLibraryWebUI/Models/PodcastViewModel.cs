using MediaLibraryDAL.DbContexts;
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
        private PodcastSort selectedPodcastSort;

        [ImportingConstructor]
        public PodcastViewModel(HomeViewModel homeViewModel)
        {
            podcastGroups = Enumerable.Empty<IGrouping<string, Podcast>>();
            this.homeViewModel = homeViewModel;
            podcastItems = Enumerable.Empty<PodcastItem>();
        }

        public IEnumerable<IGrouping<string, Podcast>> PodcastGroups { get => podcastGroups; set => podcastGroups = value; }
        public Podcast SelectedPodcast { get => selectedPodcast; set => selectedPodcast = value; }
        public IEnumerable<PodcastItem> PodcastItems { get => podcastItems; set => podcastItems = value; }
        public PodcastSort SelectedPodcastSort { get => selectedPodcastSort; set => selectedPodcastSort = value; }

        public HomeViewModel HomeViewModel { get => homeViewModel; }
    }
}