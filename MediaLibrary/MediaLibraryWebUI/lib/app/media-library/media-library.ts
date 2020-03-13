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
import DeleteModal from '../assets/modals/delete-modal';
import EditSongModal from '../assets/modals/edit-song-modal';
import { getMediaPagesEnum, getMediaPagesEnumString } from '../assets/enums/enum-functions';

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
    private deleteModal: DeleteModal;
    private editSongModal: EditSongModal;

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
        $('[data-media-page]').on('click', e => this.loadView.call(this, getMediaPagesEnum($(e.currentTarget).attr('data-media-page'))));
    }

    private load(): void {
        const success: () => void = () => {
            LoadingModal.showLoading();
            this.loadStaticViews(() => {
                LoadingModal.hideLoading();
                this.deleteModal = new DeleteModal(this.mediaLibraryConfiguration, this.loadView.bind(this));
                this.editSongModal = new EditSongModal(this.mediaLibraryConfiguration, this.loadView.bind(this));
                this.home = new Home(this.homeConfiguration);
                this.music = new Music(this.musicConfiguration, this.playWrapper.bind(this), this.updateActiveMedia.bind(this));
                this.playlist = new Playlist(this.playlistConfiguration, this.playWrapper.bind(this), this.updateActiveMedia.bind(this));
                this.podcast = new Podcast(this.podcastConfiguration, this.playWrapper.bind(this), this.updateActiveMedia.bind(this));
                this.television = new Television(this.televisionConfiguration, this.playWrapper.bind(this), this.updateActiveMedia.bind(this));
                this.player = new Player(this.playerConfiguration, {
                    loadArtist: (id) => this.music.loadArtist(id, this.loadView.bind(this, MediaPages.Music)),
                    loadAlbum: (id) => this.music.loadAlbum(id, this.loadView.bind(this, MediaPages.Music)),
                    loadPodcast: (id) => this.podcast.loadPodcast(id, this.loadView.bind(this, MediaPages.Podcast))
                },
                    this.updateActiveMedia.bind(this)
                );
                this.loadView(this.mediaLibraryConfiguration.properties.SelectedMediaPage);
            });
        };

        this.loadConfigurations(success);
    }

    private playWrapper(btn: HTMLButtonElement, playSingleItem?: boolean): void {
        this.player.play.call(this.player, btn, playSingleItem);
    }

    private updateActiveMedia(): void {
        const $mediaView: JQuery<HTMLElement> = $(this.mainViews.MediaView),
              currentId: number = this.player.getCurrentlyLoadedId();

        if (this.mediaLibraryConfiguration.properties.SelectedMediaPage === MediaPages.Music ||
            this.mediaLibraryConfiguration.properties.SelectedMediaPage === MediaPages.Playlist) {
            $mediaView.find('.list-group-item[data-song-id].active').removeClass('active');
            $mediaView.find('.list-group-item[data-song-id="' + currentId + '"]').addClass('active');
        } else if (this.mediaLibraryConfiguration.properties.SelectedMediaPage == MediaPages.Podcast ||
                   this.mediaLibraryConfiguration.properties.SelectedMediaPage == MediaPages.Television) {
            $mediaView.find('.list-group-item[data-episode-id].active').removeClass('active');
            $mediaView.find('.list-group-item[data-episode-id="' + currentId + '"]').addClass('active');
        }
    }

    private loadConfigurations(callback: () => void = () => null): void {
        $.get('Home/HomeConfiguration', data => this.homeConfiguration = Configurations.Home(data))
            .then(() => $.get('Music/MusicConfiguration', data => this.musicConfiguration = Configurations.Music(data))
                .then(() => $.get('MediaLibrary/MediaLibraryConfiguration', data => this.mediaLibraryConfiguration = Configurations.MediaLibrary(data))
                    .then(() => $.get('Television/TelevisionConfiguration', data => this.televisionConfiguration = Configurations.Television(data))
                        .then(() => $.get('Podcast/PodcastConfiguration', data => this.podcastConfiguration = Configurations.Podcast(data))
                            .then(() => $.get('Player/PlayerConfiguration', data => this.playerConfiguration = Configurations.Player(data))
                                .then(() => $.get('Playlist/PlaylistConfiguration', data => this.playlistConfiguration = Configurations.Playlist(data))
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
        const container: HTMLElement = HtmlControls.Containers().HeaderControlsContainer;

        LoadingModal.showLoading();
        $('#divNavbar').collapse('hide');
        this.mediaLibraryConfiguration.properties.SelectedMediaPage = mediaPage;
        this.disableNavItem(getMediaPagesEnumString(mediaPage));
        $(container).removeClass('d-none');
        this.mediaLibraryConfiguration.updateConfiguration(() => {
            this.prepareViews();
            this.showMainView(mediaPage);

            switch (mediaPage) {
                case MediaPages.Music:
                    this.music.loadView(() => LoadingModal.hideLoading());
                    break;
                case MediaPages.Player:
                    $(container).addClass('d-none');
                    this.player.loadView(() => LoadingModal.hideLoading());
                    break;
                case MediaPages.Playlist:
                    this.playlist.loadView(() => LoadingModal.hideLoading());
                    break;
                case MediaPages.Podcast:
                    this.podcast.loadView(() => LoadingModal.hideLoading());
                    break;
                case MediaPages.Television:
                    this.television.loadView(() => LoadingModal.hideLoading());
                    break;
                case MediaPages.Home:
                default:
                    this.home.loadView(() => LoadingModal.hideLoading());
                    break;
            }
        });
    }

    private prepareViews(): void {
        $([this.mainViews.HomeView, this.mainViews.MediaView, this.mainViews.PlayerView]).addClass('d-none');
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

    private disableNavItem(view: string): void {
        $('a.nav-link[data-media-page][href]').removeClass('d-none');
        $('a.nav-link[data-media-page]:not([href])').addClass('d-none');
        $('a.nav-link[data-media-page="' + view + '"][href]').addClass('d-none');
        $('a.nav-link[data-media-page="' + view + '"]:not([href])').removeClass('d-none');
    }
}

