using LibVLCSharp.Shared;
using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PlayerViewModel : BaseViewModel<IPlayerView>
    {
        private readonly IPlayerView playerView;
        private MediaPlayer mediaPlayer;
        private LibVLC libVLC;
        private IEnumerable<Media> mediaItems;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            Core.Initialize();
            this.playerView = playerView;
            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(libVLC) { EnableHardwareDecoding = true };
            mediaItems = Enumerable.Empty<Media>();
        }

        public MediaPlayer MediaPlayer { get => mediaPlayer; set => SetProperty<MediaPlayer>(ref mediaPlayer, value); }
        public LibVLC LibVLC { get => libVLC; set => SetProperty<LibVLC>(ref libVLC, value); }
        public IEnumerable<Media> MediaItems { get => mediaItems; set => SetProperty<IEnumerable<Media>>(ref mediaItems, value); }
    }
}
