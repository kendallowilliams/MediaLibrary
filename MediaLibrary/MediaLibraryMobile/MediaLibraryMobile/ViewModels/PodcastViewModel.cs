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
        private ICommand loadCommand;
        private bool isRefreshing;

        [ImportingConstructor]
        public PodcastViewModel(IPodcastView podcastView) : base(podcastView)
        {
            this.podcastView = podcastView;
        }

        public IEnumerable<Podcast> Podcasts { get => podcasts; set => SetProperty<IEnumerable<Podcast>>(ref podcasts, value); }
        public ICommand LoadCommand { get => loadCommand; set => SetProperty<ICommand>(ref loadCommand, value); }
        public bool IsRefreshing { get => isRefreshing; set => SetProperty<bool>(ref isRefreshing, value); }
    }
}
