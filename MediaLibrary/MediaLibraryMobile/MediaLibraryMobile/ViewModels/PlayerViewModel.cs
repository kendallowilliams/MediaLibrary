using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PlayerViewModel : BaseViewModel<IPlayerView>
    {
        private IEnumerable<string> mediaUris;
        private int? selectedPlayIndex;
        private string source;
        private bool showPlaybackControls;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            mediaUris = Enumerable.Empty<string>();
            showPlaybackControls = true;
        }

        public IEnumerable<string> MediaUris { get => mediaUris; set => SetProperty<IEnumerable<string>>(ref mediaUris, value); }
        public int? SelectedPlayIndex { get => selectedPlayIndex; set => SetProperty<int?>(ref selectedPlayIndex, value); }
        public string Source { get => source; set => SetProperty<string>(ref source, value); }
        public bool ShowPlaybackControls { get => showPlaybackControls; set => SetProperty<bool>(ref showPlaybackControls, value); }
    }
}
