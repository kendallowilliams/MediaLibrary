using Android.Media;
using LibVLCSharp.Shared;
using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class PlayerViewModel : BaseViewModel<IPlayerView>
    {
        private readonly IPlayerView playerView;
        private readonly MediaPlayer mediaPlayer;
        private readonly LibVLC libVLC;

        [ImportingConstructor]
        public PlayerViewModel(IPlayerView playerView) : base(playerView)
        {
            this.playerView = playerView;
            libVLC = new LibVLC();
            mediaPlayer = new MediaPlayer(libVLC);
        }

        public MediaPlayer MediaPlayer { get => mediaPlayer; }

        public LibVLC LibVLC { get => libVLC; }
    }
}
