import Music from './music/music';
import Player from './player/player';
import Playlist from './playlist/playlist';
import Television from './television/television';
import Podcast from './podcast/podcast';
import HtmlControls from '../assets/controls/html-controls';
import Configurations from '../assets/models/configurations/configurations';
import IBaseClass from '../assets/models/base-class';
import LoadingModal from '../assets/modals/loading-modal';
import { MediaPages } from '../assets/enums/enums';
import HomeConfiguration from '../assets/models/configurations/home-configuration';
import MediaLibraryConfiguration from '../assets/models/configurations/media-library-configuration';
import PlayerConfiguration from '../assets/models/configurations/player-configuration';
import PlaylistConfiguration from '../assets/models/configurations/playlist-configuration';
import PodcastConfiguration from '../assets/models/configurations/podcast-configuration';
import TelevisionConfiguration from '../assets/models/configurations/television-configuration';
import MusicConfiguration from '../assets/models/configurations/music-configuration';

export default class MediaLibrary extends IBaseClass {
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

    constructor() {
        super();
        this.music = new Music();
        this.player = new Player();
        this.playlist = new Playlist();
        this.podcast = new Podcast();
        this.television = new Television();

        this.load();
    }

    load(): void {
        const success = () => {
            LoadingModal.showLoading();
            $(HtmlControls.Views.PlayerView).load($(HtmlControls.Views.PlayerView).attr('data-action-url'), () => {
                LoadingModal.hideLoading();
                //this.loadView(this.mediaLibraryConfiguration.selectedMediaPage);
            });
        };
        
        this.loadConfigurations(success);
    }

    loadConfigurations(callback: () => void = () => { }): void {
        $.get('/Home/HomeConfiguration', data => this.homeConfiguration = Configurations.Home(data))
            .then(() => $.get('/Music/MusicConfiguration', data => this.musicConfiguration = Configurations.Music(data))
                .then(() => $.get('/MediaLibrary/MediaLibraryConfiguration', data => this.mediaLibraryConfiguration = Configurations.MediaLibrary(data))
                    .then(() => $.get('/Television/TelevisionConfiguration', data => this.televisionConfiguration = Configurations.Television(data))
                        .then(() => $.get('/Podcast/PodcastConfiguration', data => this.podcastConfiguration = Configurations.Podcast(data))
                            .then(() => $.get('/Player/PlayerConfiguration', data => this.playerConfiguration = Configurations.Player(data))
                                .then(() => $.get('/Playlist/PlaylistConfiguration', data => this.playlistConfiguration = Configurations.Playlist(data))
                                    .then(() => callback)
                            )
                        )
                    )
                )
            )
        );
    }

    loadView(mediaPage: MediaPages): void {

    }
}

