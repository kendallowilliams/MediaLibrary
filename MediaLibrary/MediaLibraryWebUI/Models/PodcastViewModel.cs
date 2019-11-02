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
        [ImportingConstructor]
        public PodcastViewModel(HomeViewModel homeViewModel)
        {
            PodcastGroups = Enumerable.Empty<IGrouping<string, Podcast>>();
            this.HomeViewModel = homeViewModel;
            PodcastItems = Enumerable.Empty<PodcastItem>();
            Configuration = new PodcastConfiguration();
            DownloadedEpisodes = Enumerable.Empty<int>();
        }

        public IEnumerable<IGrouping<string, Podcast>> PodcastGroups { get; set; }
        public Podcast SelectedPodcast { get; set; }
        public IEnumerable<PodcastItem> PodcastItems { get; set; }
        public HomeViewModel HomeViewModel { get; }
        public PodcastConfiguration Configuration { get; set; }
        public IEnumerable<int> DownloadedEpisodes { get; set; }
    }
}