import Music from './music/music';
import Player from './player/player';
import Playlist from './playlist/playlist';
import Television from './television/television';
import Podcast from './podcast/podcast';
import HtmlControls from '../assets/controls/html-controls';
import Configurations from '../assets/models/configurations/configurations';
import BaseClass from '../assets/models/base-class';
import LoadingModal from '../assets/modals/loading-modal';
import { MediaPages } from '../assets/enums/enums';
import HomeConfiguration from '../assets/models/configurations/home-configuration';
import MediaLibraryConfiguration from '../assets/models/configurations/media-library-configuration';
import PlayerConfiguration from '../assets/models/configurations/player-configuration';
import PlaylistConfiguration from '../assets/models/configurations/playlist-configuration';
import PodcastConfiguration from '../assets/models/configurations/podcast-configuration';
import TelevisionConfiguration from '../assets/models/configurations/television-configuration';
import MusicConfiguration from '../assets/models/configurations/music-configuration';
import Home from './home/home';

export default class MediaLibrary extends BaseClass {
    private home: Home;
    private music: Music;
    private player: Player;
    private playlist: Playlist;
    private television: Television;
    private podcast: Podcast;
    private homeConfiguration: HomeConfiguration;
    private mediaLibraryConfiguration: MediaLibraryConfiguration;
    private playerConfiguration: PlayerConfiguration;
    private playlistConfiguration: PlaylistConfiguration;
    private podcastConfiguration: PodcastConfiguration;
    private televisionConfiguration: TelevisionConfiguration;
    private musicConfiguration: MusicConfiguration;
    private mainViews: { HomeView: HTMLElement, MediaView: HTMLElement, PlayerView: HTMLElement };

    constructor() {
        super();
        this.initialize();
        this.load();
        this.mainViews = HtmlControls.Views();
    }

    private initialize(): void {
        this.initializeControls();
    }

    private initializeControls(): void {
        $('[data-media-page]').on('click', e => this.loadView.call(this, this.getMediaPagesEnum($(e.target).attr('data-media-page'))));
    }

    private load(): void {
        const success: () => void = () => {
            LoadingModal.showLoading();
            this.loadStaticViews(() => {
                LoadingModal.hideLoading();
                this.home = new Home(this.homeConfiguration);
                this.player = new Player(this.playerConfiguration);
                this.music = new Music(this.musicConfiguration, this.player.play);
                this.playlist = new Playlist(this.playlistConfiguration);
                this.podcast = new Podcast(this.podcastConfiguration);
                this.television = new Television(this.televisionConfiguration);
                this.loadView(this.mediaLibraryConfiguration.properties.SelectedMediaPage);
            });
        };

        this.loadConfigurations(success);
    }

    private loadConfigurations(callback: () => void = () => null): void {
        $.get('/Home/HomeConfiguration', data => this.homeConfiguration = Configurations.Home(data))
            .then(() => $.get('/Music/MusicConfiguration', data => this.musicConfiguration = Configurations.Music(data))
                .then(() => $.get('/MediaLibrary/MediaLibraryConfiguration', data => this.mediaLibraryConfiguration = Configurations.MediaLibrary(data))
                    .then(() => $.get('/Television/TelevisionConfiguration', data => this.televisionConfiguration = Configurations.Television(data))
                        .then(() => $.get('/Podcast/PodcastConfiguration', data => this.podcastConfiguration = Configurations.Podcast(data))
                            .then(() => $.get('/Player/PlayerConfiguration', data => this.playerConfiguration = Configurations.Player(data))
                                .then(() => $.get('/Playlist/PlaylistConfiguration', data => this.playlistConfiguration = Configurations.Playlist(data))
                                    .then(callback)
                            )
                        )
                    )
                )
            )
        );
    }

    private loadStaticViews(callback: () => void = () => null) {
        $(this.mainViews.PlayerView).load($(this.mainViews.PlayerView).attr('data-action-url'), () => {
            $(this.mainViews.HomeView).load($(this.mainViews.HomeView).attr('data-action-url'), callback);
        });
    }

    private loadView(mediaPage: MediaPages): void {
        LoadingModal.showLoading();
        this.mediaLibraryConfiguration.properties.SelectedMediaPage = mediaPage;
        this.mediaLibraryConfiguration.updateConfiguration(() => {
            this.prepareViews();
            LoadingModal.hideLoading();
            this.showMainView(mediaPage);

            switch (mediaPage) {
                case MediaPages.Music:
                    this.music.loadView();
                    break;
                case MediaPages.Player:
                    this.player.loadView();
                    break;
                case MediaPages.Playlist:
                    this.playlist.loadView();
                    break;
                case MediaPages.Podcast:
                    this.podcast.loadView();
                    break;
                case MediaPages.Television:
                    this.television.loadView();
                    break;
                case MediaPages.Home:
                default:
                    this.home.loadView();
                    break;
            }
        });
    }

    private prepareViews(): void {
        const headerContainer = HtmlControls.Containers().HeaderControlsContainer;

        $([this.mainViews.HomeView, this.mainViews.MediaView, this.mainViews.PlayerView, headerContainer]).addClass('d-none');
    }

    private showMainView(mediaPage: MediaPages): void {
        switch (mediaPage) {
            case MediaPages.Home:
                $(this.mainViews.HomeView).removeClass('d-none');
                break;
            case MediaPages.Player:
                $(this.mainViews.PlayerView).removeClass('d-none');
                break;
            default:
                $(this.mainViews.MediaView).removeClass('d-none');
                break;

        }
    }

    private getMediaPagesEnum(page: string): MediaPages {
        let mediaPage: MediaPages;

        switch (page) {
            case 'Music':
                mediaPage = MediaPages.Music;
                break;
            case 'Playlist':
                mediaPage = MediaPages.Playlist;
                break;
            case 'Player':
                mediaPage = MediaPages.Player;
                break;
            case 'Podcast':
                mediaPage = MediaPages.Podcast;
                break;
            case 'Television':
                mediaPage = MediaPages.Television;
                break;
            case 'Home':
            default:
                mediaPage = MediaPages.Home;
                break;
        }

        return mediaPage;
    }
}

