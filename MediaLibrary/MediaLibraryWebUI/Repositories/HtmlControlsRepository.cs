using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediaLibraryWebUI.Repositories
{
    public class HtmlControlsRepository
    {
        #region View Ids
        public static string HomeViewId { get => "home-view"; }
        public static string PlayerViewId { get => "player-view"; }
        public static string MediaViewId { get => "media-view"; }
        public static string PodcastViewId { get => "podcast-view"; }
        public static string SeasonViewId { get => "season-view"; }
        #endregion View Ids

        #region Player Ids
        public static string MusicPlayerId { get => "music-player"; }
        public static string VideoPlayerId { get => "video-player"; }
        #endregion Player Ids

        #region Button Ids
        public static string HeaderControlsContainerId { get => "header-controls-container"; }
        public static string HeaderPlayButtonId { get => "btn-header-play"; }
        public static string HeaderPreviousButtonId { get => "btn-header-previous"; }
        public static string HeaderNextButtonId { get => "btn-header-next"; }
        public static string HeaderPauseButtonId { get => "btn-header-pause"; }
        public static string HeaderShuffleButtonId { get => "btn-header-shuffle"; }
        public static string HeaderRepeatButtonId { get => "btn-header-repeat"; }
        public static string HeaderRepeatOneButtonId { get => "btn-header-repeat-one"; }
        public static string HeaderRepeatAllButtonId { get => "btn-header-repeat-all"; }
        public static string PlayerPlayButtonId { get => "btn-player-play"; }
        public static string PlayerPreviousButtonId { get => "btn-player-previous"; }
        public static string PlayerNextButtonId { get => "btn-player-next"; }
        public static string PlayerPauseButtonId { get => "btn-player-pause"; }
        public static string PlayerShuffleButtonId { get => "btn-player-shuffle"; }
        public static string PlayerRepeatButtonId { get => "btn-player-repeat"; }
        public static string PlayerRepeatOneButtonId { get => "btn-player-repeat-one"; }
        public static string PlayerRepeatAllButtonId { get => "btn-player-repeat-all"; }
        public static string PlayerPlaylistToggleButtonId { get => "btn-player-playlist-toggle"; }
        public static string PlayerVolumeButtonId { get => "btn-player-volume"; }
        public static string PlayerMuteButtonId { get => "btn-player-mute"; }
        public static string PlayerFullscreenButtonId { get => "btn-player-fullscreen"; }
        public static string PlayerClearButtonId { get => "btn-player-clear"; }
        public static string PlayerAudioVisualizerButtonId { get => "btn-audio-visualizer"; }
        #endregion Button Ids

        #region Container Ids
        public static string PlayerVideoContainerId { get => "video-container"; }
        public static string PlayerAudioContainerId { get => "audio-container"; }
        public static string PlayerItemsContainerId { get => "player-items-container"; }
        public static string PlayerVolumeContainerId { get => "player-volume-container"; }
        public static string SongsContainerId { get => "songs-container"; }
        public static string ArtistsContainerId { get => "artists-container"; }
        public static string AlbumsContainerId { get => "albums-container"; }
        #endregion Container Ids

        #region UI Control Ids
        public static string PlayerSliderId { get => "player-slider"; }
        public static string VolumeSliderId { get => "volume-slider"; }
        public static string PlayerTimeId { get => "player-time"; }
        public static string AudioVisualizerId { get => "audio-visualizer"; }
        public static string MusicTabListId { get => "music-tab-list"; }
        #endregion UI Control Ids

        #region UI Field Ids
        public static string NowPlayingTitleId { get => "player-title"; }
        #endregion UI Field Ids

        #region Modal Ids
        public static string NewPlaylistModalId { get => "new-playlist-modal"; }
        public static string NewSongModalId { get => "new-song-modal"; }
        public static string NewPodcastModalId { get => "new-podcast-modal"; }
        public static string AddToPlaylistModalId { get => "add-to-playlist-modal"; }
        public static string DeleteModalId { get => "delete-modal"; }
        public static string EdiPlaylistModalId { get => "edit-playlist-modal"; }
        public static string EditSongModalId { get => "edit-song-modal"; }
        public static string LoadingModalId { get => "loading-modal"; }
        #endregion Modal Ids
    }
}