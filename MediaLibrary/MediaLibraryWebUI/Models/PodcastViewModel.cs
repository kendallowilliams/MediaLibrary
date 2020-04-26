using MediaLibraryDAL.DbContexts;
using MediaLibraryWebUI.Models.Configurations;
using MediaLibraryWebUI.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MediaLibraryWebUI.UIEnums;

namespace MediaLibraryWebUI.Models
{
    [Export]
    public class PodcastViewModel : ViewModel<PodcastConfiguration>
    {
        [ImportingConstructor]
        public PodcastViewModel()
        {
            PodcastGroups = Enumerable.Empty<IGrouping<string, Podcast>>();
            PodcastItems = Enumerable.Empty<PodcastItem>();
            PodcastSortItems = PodcastRepository.GetPodcastSortItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
            PodcastFilterItems = PodcastRepository.GetPodcastFilterItems().Select(item => new SelectListItem { Text = item.Name, Value = item.Value.ToString() });
        }

        public IEnumerable<IGrouping<string, Podcast>> PodcastGroups { get; set; }
        public Podcast SelectedPodcast { get; set; }
        public IEnumerable<PodcastItem> PodcastItems { get; set; }
        public IEnumerable<SelectListItem> PodcastSortItems { get; }
        public IEnumerable<SelectListItem> PodcastFilterItems { get; }
        public bool HasPlaylists { get; set; }
    }
}