using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using MediaLibraryDAL.DbContexts;
using MediaLibraryMobile.Views.Interfaces;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PodcastViewModel : BaseViewModel<IPodcastsView>
    {
        private ObservableCollection<Podcast> podcasts;
        private ObservableCollection<PodcastItem> selectedPodcastItems;
        private ICommand loadPodcastsCommand,
                         loadPodcastCommand,
                         playCommand,
                         playerCommand;
        private bool isRefreshing;
        private Podcast selectedPodcast;
        private readonly IPodcastView podcastView;

        [ImportingConstructor]
        public PodcastViewModel(IPodcastsView podcastsView, IPodcastView podcastView) : base(podcastsView)
        {
            this.podcastView = podcastView;
            this.podcastView.BindingContext = this;
            Podcasts = new ObservableCollection<Podcast>();
            SelectedPodcastItems = new ObservableCollection<PodcastItem>();
        }

        public ObservableCollection<Podcast> Podcasts { get => podcasts; set => SetProperty<ObservableCollection<Podcast>>(ref podcasts, value); }
        public ICommand LoadPodcastsCommand { get => loadPodcastsCommand; set => SetProperty<ICommand>(ref loadPodcastsCommand, value); }
        public ICommand PlayerCommand { get => playerCommand; set => SetProperty<ICommand>(ref playerCommand, value); }
        public ICommand PlayCommand { get => playCommand; set => SetProperty<ICommand>(ref playCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
        public Podcast SelectedPodcast { get => selectedPodcast; set => SetProperty<Podcast>(ref selectedPodcast, value); }
        public ObservableCollection<PodcastItem> SelectedPodcastItems { get => selectedPodcastItems; set => SetProperty<ObservableCollection<PodcastItem>>(ref selectedPodcastItems, value); }
        public ICommand LoadPodcastCommand { get => loadPodcastCommand; set => SetProperty<ICommand>(ref loadPodcastCommand, value); }
        public IPodcastView PodcastView { get => podcastView; }
    }
}
