using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PodcastViewModel
    {
        private IEnumerable<IGrouping<string, Podcast>> podcastGroups;
        private readonly HomeViewModel homeViewModel;
        private Podcast selectedPodcast;

        [ImportingConstructor]
        public PodcastViewModel(HomeViewModel homeViewModel)
        {
            podcastGroups = Enumerable.Empty<IGrouping<string, Podcast>>();
            this.homeViewModel = homeViewModel;
        }

        public IEnumerable<IGrouping<string, Podcast>> PodcastGroups { get => podcastGroups; set => podcastGroups = value; }
        public Podcast SelectedPodcast { get => selectedPodcast; set => selectedPodcast = value; }
    }
}