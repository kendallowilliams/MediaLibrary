using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryDAL.DbContexts;
using MediaLibraryMobile.Views.Interfaces;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PodcastViewModel : BaseViewModel<IPodcastView>
    {
        private readonly IPodcastView podcastView;
        private IEnumerable<Podcast> podcasts;

        [ImportingConstructor]
        public PodcastViewModel(IPodcastView podcastView): base(podcastView)
        {
            this.podcastView = podcastView;
        }

        public IEnumerable<Podcast> Podcasts { get => podcasts; set => SetProperty<IEnumerable<Podcast>>(ref podcasts, value); }
    }
}
