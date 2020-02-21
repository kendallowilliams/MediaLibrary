export default {
    Views: () => ({
        HomeView: document.getElementById('home-view'),
        PlayerView: document.getElementById('player-view'),
        MediaView: document.getElementById('media-view'),
        PodcastView: document.getElementById('podcast-view'),
        SeasonView: document.getElementById('season-view'),
    }),
    Players: () => ({
        MusicPlayer: document.getElementById('music-player') as HTMLMediaElement,
        VideoPlayer: document.getElementById('video-player') as HTMLMediaElement
    }),
    Buttons: () => ({
        HeaderPlayButton: document.getElementById('btn-header-play'),
        HeaderPreviousButton: document.getElementById('btn-header-previous'),
        HeaderNextButton: document.getElementById('btn-header-next'),
        HeaderPauseButton: document.getElementById('btn-header-pause'),
        HeaderShuffleButton: document.getElementById('btn-header-shuffle'),
        HeaderRepeatButton: document.getElementById('btn-header-repeat'),
        HeaderRepeatOneButton: document.getElementById('btn-header-repeat-one'),
        HeaderRepeatAllButton: document.getElementById('btn-header-repeat-all'),
        PlayerPlayButton: document.getElementById('btn-player-play'),
        PlayerPreviousButton: document.getElementById('btn-player-previous'),
        PlayerNextButton: document.getElementById('btn-player-next'),
        PlayerPauseButton: document.getElementById('btn-player-pause'),
        PlayerShuffleButton: document.getElementById('btn-player-shuffle'),
        PlayerRepeatButton: document.getElementById('btn-player-repeat'),
        PlayerRepeatOneButton: document.getElementById('btn-player-repeat-one'),
        PlayerRepeatAllButton: document.getElementById('btn-player-repeat-all'),
        PlayerPlaylistToggleButton: document.getElementById('btn-player-playlist-toggle'),
        PlayerVolumeButton: document.getElementById('btn-player-volume'),
        PlayerMuteButton: document.getElementById('btn-player-mute'),
        PlayerFullscreenButton: document.getElementById('btn-player-fullscreen')
    }),
    Containers: () => ({
        HeaderControlsContainer: document.getElementById('header-controls-container'),
        PlayerVideoContainer: document.getElementById('video-container'),
        PlayerAudioContainer: document.getElementById('audio-container'),
        PlayerItemsContainer: document.getElementById('player-items-container'),
        PlayerVolumeContainer: document.getElementById('player-volume-container'),
        SongsContainer: document.getElementById('songs-container'),
        ArtistsContainer: document.getElementById('artists-container'),
        AlbumsContainer: document.getElementById('albums-container')
    }),
    UIControls: () => ({
        PlayerSlider: document.getElementById('player-slider'),
        VolumeSlider: document.getElementById('volume-slider'),
        PlayerTime: document.getElementById('player-time'),
        AudioVisualizer: document.getElementById('audio-visualizer') as HTMLCanvasElement,
        MusicTabList: document.getElementById('music-tab-list')
    }),
    UIFields: () => ({
        NowPlayingTitle: document.getElementById('player-title')
    }),
    Modals: () => ({
        NewPlaylistModal: document.getElementById('new-playlist-modal'),
        NewSongModal: document.getElementById('new-song-modal'),
        AddToPlaylistModal: document.getElementById('add-to-playlist-modal'),
        DeleteModal: document.getElementById('delete-modal'),
        EdiPlaylistModal: document.getElementById('edit-playlist-modal'),
        EditSongModal: document.getElementById('edit-song-modal'),
        LoadingModal: document.getElementById('loading-modal')
    })
}