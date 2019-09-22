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
        private IEnumerable<Podcast> podcasts;
        private readonly HomeViewModel homeViewModel;

        [ImportingConstructor]
        public PodcastViewModel(HomeViewModel homeViewModel)
        {
            podcasts = Enumerable.Empty<Podcast>();
            this.homeViewModel = homeViewModel;
        }

        public IEnumerable<Podcast> Podcasts { get => podcasts; set => podcasts = value; }
    }
}