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
    }
}