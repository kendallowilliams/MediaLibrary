using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Repositories
{
    public class HtmlControlsRepository
    {
        public static string MusicPlayerId { get => "music-player"; }
        public static string VideoPlayerId { get => "video-player"; }
        public static string HeaderPlayButtonId { get => "btn-header-play"; }
        public static string HeaderPreviousButtonId { get => "btn-header-previous"; }
        public static string HeaderNextButtonId { get => "btn-header-next"; }
        public static string HeaderPauseButtonId { get => "btn-header-pause"; }
        public static string HeaderShuffleButtonId { get => "btn-header-shuffle"; }
        public static string HeaderRepeatButtonId { get => "btn-header-repeat"; }
        public static string PlayerVideoContainerId { get => "video-container"; }
        public static string PlayerAudioContainerId { get => "audio-container"; }
        public static string PlayerItemsContainerId { get => "player-items-container"; }
        public static string PlayerPlayButtonId { get => "btn-player-play"; }
        public static string PlayerPreviousButtonId { get => "btn-player-previous"; }
        public static string PlayerNextButtonId { get => "btn-player-next"; }
        public static string PlayerPauseButtonId { get => "btn-player-pause"; }
        public static string PlayerShuffleButtonId { get => "btn-player-shuffle"; }
        public static string PlayerRepeatButtonId { get => "btn-player-repeat"; }
        public static string PlayerPlaylistToggleButtonId { get => "btn-player-playlist-toggle"; }
    }
}