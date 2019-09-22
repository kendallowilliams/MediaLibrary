using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public class PodcastViewModel
    {
        private IEnumerable<IGrouping<string, Podcast>> podcasts;

        public PodcastViewModel()
        {
            podcasts = Enumerable.Empty<IGrouping<string, Podcast>>();
        }

        public IEnumerable<IGrouping<string, Podcast>> Podcasts { get => podcasts; set => podcasts = value; }
    }
}