using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
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
        private ICommand mediaEndedCommand,
                         nextCommand,
                         previousCommand;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            mediaUris = Enumerable.Empty<string>();
            ShowPlaybackControls = true;
        }

        public IEnumerable<string> MediaUris { get => mediaUris; set => SetProperty<IEnumerable<string>>(ref mediaUris, value); }
        public int? SelectedPlayIndex { get => selectedPlayIndex; set => SetProperty<int?>(ref selectedPlayIndex, value); }
        public string Source { get => source; set => SetProperty<string>(ref source, value); }
        public bool ShowPlaybackControls { get => showPlaybackControls; set => SetProperty<bool>(ref showPlaybackControls, value); }
        public ICommand MediaEndedCommand { get => mediaEndedCommand; set => SetProperty<ICommand>(ref mediaEndedCommand, value); }
        public ICommand PreviousCommand { get => previousCommand; set => SetProperty<ICommand>(ref previousCommand, value); }
        public ICommand NextCommand { get => nextCommand; set => SetProperty<ICommand>(ref nextCommand, value); }
    }
}
