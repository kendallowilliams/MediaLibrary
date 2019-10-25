using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MediaLibraryWebUI.Enums;

namespace MediaLibraryWebUI.Models.Configurations
{
    public class PodcastConfiguration
    {
        private int selectedPodcastId;
        private PodcastPages selectedPodcastPage;
        private PodcastSort selectedPodcastSort;

        public PodcastConfiguration()
        {
        }

        public int SelectedPodcastId { get => selectedPodcastId; set => selectedPodcastId = value; }
        public PodcastPages SelectedPodcastPage { get => selectedPodcastPage; set => selectedPodcastPage = value; }
        public PodcastSort SelectedPodcastSort { get => selectedPodcastSort; set => selectedPodcastSort = value; }
    }
}