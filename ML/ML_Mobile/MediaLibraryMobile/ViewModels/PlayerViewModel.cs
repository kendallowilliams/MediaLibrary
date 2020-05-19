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
        private IEnumerable<(int, string, Uri)> mediaItems;
        private int? selectedPlayIndex;
        private MediaPlayer mediaPlayer;
        private LibVLC libVLC;
        private bool showPlaybackControls,
                     isPlaying,
                     isRandom,
                     automaticallyHideControls;
        private ICommand nextCommand,
                         previousCommand,
                         randomCommand;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            Core.Initialize();

            mediaItems = Enumerable.Empty<(int, string, Uri)>();
            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(LibVLC) { EnableHardwareDecoding = true };
            ShowPlaybackControls = true;
            AutomaticallyHideControls = false;
        }

        public IEnumerable<(int Id, string Title, Uri Uri)> MediaItems { get => mediaItems; set => SetProperty<IEnumerable<(int, string, Uri)>>(ref mediaItems, value); }
        public int? SelectedPlayIndex { get => selectedPlayIndex; set => SetProperty<int?>(ref selectedPlayIndex, value); }
        public bool ShowPlaybackControls { get => showPlaybackControls; set => SetProperty<bool>(ref showPlaybackControls, value); }
        public bool IsPlaying { get => isPlaying; set => SetProperty<bool>(ref isPlaying, value); }
        public bool IsRandom { get => isRandom; set => SetProperty<bool>(ref isRandom, value); }
        public bool AutomaticallyHideControls { get => automaticallyHideControls; set => SetProperty(ref automaticallyHideControls, value); }
        public ICommand PreviousCommand { get => previousCommand; set => SetProperty<ICommand>(ref previousCommand, value); }
        public ICommand NextCommand { get => nextCommand; set => SetProperty<ICommand>(ref nextCommand, value); }
        public ICommand RandomCommand { get => randomCommand; set => SetProperty<ICommand>(ref randomCommand, value); }
        public MediaPlayer MediaPlayer { get => mediaPlayer; set => SetProperty<MediaPlayer>(ref mediaPlayer, value); }
        public LibVLC LibVLC { get => libVLC; set => SetProperty<LibVLC>(ref libVLC, value); }
        public double CurrentPosition { get => Math.Floor(mediaPlayer.Position * mediaPlayer.Length / 1000); }
    }
}
