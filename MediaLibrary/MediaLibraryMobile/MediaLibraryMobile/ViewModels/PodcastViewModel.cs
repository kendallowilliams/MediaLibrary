using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryDAL.DbContexts;
using MediaLibraryMobile.Views.Interfaces;
using System.Windows.Input;
using System.Linq;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PodcastViewModel : BaseViewModel<IPodcastsView>
    {
        private readonly IPodcastsView podcastsView;
        private IEnumerable<Podcast> podcasts;
        private ICommand loadPodcastsCommand;
        private ICommand loadPodcastCommand;
        private bool isRefreshing;
        private Podcast selectedPodcast;
        private readonly IPodcastView podcastView;

        [ImportingConstructor]
        public PodcastViewModel(IPodcastsView podcastsView, IPodcastView podcastView) : base(podcastsView)
        {
            this.podcastsView = podcastsView;
            this.podcastView = podcastView;
            this.podcastView.BindingContext = this;
            podcasts = Enumerable.Empty<Podcast>();
        }

        public IEnumerable<Podcast> Podcasts { get => podcasts; set => SetProperty<IEnumerable<Podcast>>(ref podcasts, value); }
        public ICommand LoadPodcastsCommand { get => loadPodcastsCommand; set => SetProperty<ICommand>(ref loadPodcastsCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
        public Podcast SelectedPodcast { get => selectedPodcast; set => SetProperty<Podcast>(ref selectedPodcast, value); }
        public ICommand LoadPodcastCommand { get => loadPodcastCommand; set => SetProperty<ICommand>(ref loadPodcastCommand, value); }
        public IPodcastView PodcastView { get => podcastView; }
    }
}
