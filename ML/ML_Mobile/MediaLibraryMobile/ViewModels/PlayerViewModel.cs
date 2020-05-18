using LibVLCSharp.Shared;
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
        private IEnumerable<Uri> mediaUris;
        private int? selectedPlayIndex;
        private MediaPlayer mediaPlayer;
        private LibVLC libVLC;
        private bool showPlaybackControls,
                     isPlaying,
                     automaticallyHideControls;
        private ICommand nextCommand,
                         previousCommand;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            Core.Initialize();

            mediaUris = Enumerable.Empty<Uri>();
            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(LibVLC) { EnableHardwareDecoding = true };
            ShowPlaybackControls = true;
            IsPlaying = true;
            AutomaticallyHideControls = false;
        }

        public IEnumerable<Uri> MediaUris { get => mediaUris; set => SetProperty<IEnumerable<Uri>>(ref mediaUris, value); }
        public int? SelectedPlayIndex { get => selectedPlayIndex; set => SetProperty<int?>(ref selectedPlayIndex, value); }
        public bool ShowPlaybackControls { get => showPlaybackControls; set => SetProperty<bool>(ref showPlaybackControls, value); }
        public bool IsPlaying { get => isPlaying; set => SetProperty<bool>(ref isPlaying, value); }
        public bool AutomaticallyHideControls { get => automaticallyHideControls; set => SetProperty(ref automaticallyHideControls, value); }
        public ICommand PreviousCommand { get => previousCommand; set => SetProperty<ICommand>(ref previousCommand, value); }
        public ICommand NextCommand { get => nextCommand; set => SetProperty<ICommand>(ref nextCommand, value); }
        public MediaPlayer MediaPlayer { get => mediaPlayer; set => SetProperty<MediaPlayer>(ref mediaPlayer, value); }
        public LibVLC LibVLC { get => libVLC; set => SetProperty<LibVLC>(ref libVLC, value); }
        public double CurrentPosition { get => Math.Floor(mediaPlayer.Position * mediaPlayer.Length / 1000); }
    }
}
