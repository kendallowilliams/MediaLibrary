using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Models
{
    public class PodcastViewModel
    {
        private IEnumerable<Podcast> podcasts;

        public PodcastViewModel()
        {
            podcasts = Enumerable.Empty<Podcast>();
        }

        public IEnumerable<Podcast> Podcasts { get => podcasts; set => podcasts = value; }
    }
}