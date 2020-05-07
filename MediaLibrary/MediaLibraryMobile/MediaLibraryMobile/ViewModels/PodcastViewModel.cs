using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryDAL.DbContexts;
using MediaLibraryMobile.Views.Interfaces;
using System.Windows.Input;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PodcastViewModel : BaseViewModel<IPodcastView>
    {
        private readonly IPodcastView podcastView;
        private IEnumerable<Podcast> podcasts;
        private ICommand loadPodcastsCommand;
        private ICommand loadPodcastCommand;
        private bool isRefreshing;
        private Podcast selectedPodcast;

        [ImportingConstructor]
        public PodcastViewModel(IPodcastView podcastView) : base(podcastView)
        {
            this.podcastView = podcastView;
        }

        public IEnumerable<Podcast> Podcasts { get => podcasts; set => SetProperty<IEnumerable<Podcast>>(ref podcasts, value); }
        public ICommand LoadPodcastsCommand { get => loadPodcastsCommand; set => SetProperty<ICommand>(ref loadPodcastsCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
        public Podcast SelectedPodcast { get => selectedPodcast; set => SetProperty<Podcast>(ref selectedPodcast, value); }
        public ICommand LoadPodcastCommand { get => loadPodcastCommand; set => SetProperty<ICommand>(ref loadPodcastCommand, value); }
    }
}
