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
        private MediaPlayer mediaPlayer;
        private LibVLC libVLC;
        private IEnumerable<Uri> mediaUris;
        private int? selectedPlayIndex;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            Core.Initialize();
            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(libVLC) { EnableHardwareDecoding = true };
            mediaUris = Enumerable.Empty<Uri>();
        }

        public MediaPlayer MediaPlayer { get => mediaPlayer; set => SetProperty<MediaPlayer>(ref mediaPlayer, value); }
        public LibVLC LibVLC { get => libVLC; set => SetProperty<LibVLC>(ref libVLC, value); }
        public IEnumerable<Uri> MediaUris { get => mediaUris; set => SetProperty<IEnumerable<Uri>>(ref mediaUris, value); }
        public int? SelectedPlayIndex { get => selectedPlayIndex; set => SetProperty<int?>(ref selectedPlayIndex, value); }
    }
}
