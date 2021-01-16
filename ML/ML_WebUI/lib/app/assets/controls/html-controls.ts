export default {
    Views: () => ({
        HomeView: document.getElementById('home-view'),
        PlayerView: document.getElementById('player-view'),
        MediaView: document.getElementById('media-view'),
        PodcastView: document.getElementById('podcast-view'),
        SeasonView: document.getElementById('season-view')
    }),
    Players: () => ({
        MusicPlayer: document.getElementById('music-player') as HTMLMediaElement,
        VideoPlayer: document.getElementById('video-player') as HTMLMediaElement
    }),
    Buttons: () => ({
        HeaderPlayButton: document.getElementById('btn-header-play'),
        HeaderPreviousButton: document.getElementById('btn-header-previous'),
        HeaderBackwardButton: document.getElementById('btn-header-backward'),
        HeaderForwardButton: document.getElementById('btn-header-forward'),
        HeaderNextButton: document.getElementById('btn-header-next'),
        HeaderPauseButton: document.getElementById('btn-header-pause'),
        HeaderShuffleButton: document.getElementById('btn-header-shuffle'),
        HeaderRepeatButton: document.getElementById('btn-header-repeat'),
        HeaderRepeatOneButton: document.getElementById('btn-header-repeat-one'),
        HeaderRepeatAllButton: document.getElementById('btn-header-repeat-all'),
        PlayerPlayButton: document.getElementById('btn-player-play'),
        PlayerPreviousButton: document.getElementById('btn-player-previous'),
        PlayerBackwardButton: document.getElementById('btn-player-backward'),
        PlayerForwardButton: document.getElementById('btn-player-forward'),
        PlayerNextButton: document.getElementById('btn-player-next'),
        PlayerPauseButton: document.getElementById('btn-player-pause'),
        PlayerShuffleButton: document.getElementById('btn-player-shuffle'),
        PlayerRepeatButton: document.getElementById('btn-player-repeat'),
        PlayerRepeatOneButton: document.getElementById('btn-player-repeat-one'),
        PlayerRepeatAllButton: document.getElementById('btn-player-repeat-all'),
        PlayerPlaylistToggleButton: document.getElementById('btn-player-playlist-toggle'),
        PlayerVolumeButton: document.getElementById('btn-player-volume'),
        PlayerMuteButton: document.getElementById('btn-player-mute'),
        PlayerFullscreenButton: document.getElementById('btn-player-fullscreen'),
        PlayerClearButton: document.getElementById('btn-player-clear'),
        PlayerAudioVisualizerButton: document.getElementById('btn-audio-visualizer')
    }),
    Containers: () => ({
        HeaderControlsContainer: document.getElementById('header-controls-container'),
        PlayerVideoContainer: document.getElementById('video-container'),
        PlayerAudioContainer: document.getElementById('audio-container'),
        PlayerItemsContainer: document.getElementById('player-items-container'),
        PlayerVolumeContainer: document.getElementById('player-volume-container'),
        SongsContainer: document.getElementById('songs-container'),
        ArtistsContainer: document.getElementById('artists-container'),
        AlbumsContainer: document.getElementById('albums-container'),
        MusicPlaylistContainer: document.getElementById('music-playlist-container'),
        PodcastPlaylistContainer: document.getElementById('podcast-playlist-container'),
        EpisodePlaylistContainer: document.getElementById('episode-playlist-container'),
        SearchSongsContainer: document.getElementById('search-songs-container'),
        SearchArtistsContainer: document.getElementById('search-artists-container'),
        SearchAlbumsContainer: document.getElementById('search-albums-container')
    }),
    UIControls: () => ({
        PlayerSlider: document.getElementById('player-slider'),
        VolumeSlider: document.getElementById('volume-slider'),
        PlayerTime: document.getElementById('player-time'),
        AudioVisualizer: document.getElementById('audio-visualizer') as HTMLCanvasElement,
        MusicTabList: document.getElementById('music-tab-list'),
        PlaylistTabList: document.getElementById('playlist-tab-list'),
        SearchQuery: document.getElementById('search-query') as HTMLInputElement
    }),
    UIFields: () => ({
        NowPlayingTitle: document.getElementById('player-title')
    }),
    Modals: () => ({
        NewPlaylistModal: document.getElementById('new-playlist-modal'),
        NewSongModal: document.getElementById('new-song-modal'),
        NewPodcastModal: document.getElementById('new-podcast-modal'),
        AddToPlaylistModal: document.getElementById('add-to-playlist-modal'),
        DeleteModal: document.getElementById('delete-modal'),
        EdiPlaylistModal: document.getElementById('edit-playlist-modal'),
        EditSongModal: document.getElementById('edit-song-modal'),
        LoadingModal: document.getElementById('loading-modal'),
        ClearNowPlayingModal: document.getElementById('clear-now-playing-modal'),
        DownloadM3UPlaylistModal: document.getElementById('download-m3u-playlist-modal'),
        ManageDirectoriesModal: document.getElementById('manage-directories-modal'),
        AlertModal: document.getElementById('alert-modal'),
        ErrorModal: document.getElementById('error-modal'),
        ConfirmModal: document.getElementById('confirm-modal'),
        WarningModal: document.getElementById('warning-modal')
    })
}