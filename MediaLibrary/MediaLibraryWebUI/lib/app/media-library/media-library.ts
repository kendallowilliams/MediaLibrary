import Music from './music/music';
import Player from './player/player';
import Playlist from './playlist/playlist';
import Television from './television/television';
import Podcast from './podcast/podcast';
import HtmlControls from '../assets/controls/html-controls';
import Configurations from '../assets/models/configurations/configurations';
import IBaseClass from '../assets/models/base-class'
import LoadingModal from '../assets/modals/loading-modal'

export default class MediaLibrary extends IBaseClass {
    private music: Music;
    private player: Player;
    private playlist: Playlist;
    private television: Television;
    private podcast: Podcast;

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
                //loadView('@(Model.Configuration.SelectedMediaPage.ToString())');
            });
        };

        this.loadConfigurations(success);
    }

    loadConfigurations(callback: () => void = () => { }): void {
        const containers = HtmlControls.Containers;

        $(containers.HomeConfigurationContainer).load($(containers.HomeConfigurationContainer).attr('data-action-url'), function () {
            $(containers.MusicConfigurationContainer).load($(containers.MusicConfigurationContainer).attr('data-action-url'), function () {
                $(containers.MediaLibraryConfigurationContainer).load($(containers.MediaLibraryConfigurationContainer).attr('data-action-url'), function () {
                    $(containers.PodcastConfigurationContainer).load($(containers.PodcastConfigurationContainer).attr('data-action-url'), function () {
                        $(containers.PlayerConfigurationContainer).load($(containers.PlayerConfigurationContainer).attr('data-action-url'), function () {
                            $(containers.PlaylistConfigurationContainer).load($(containers.PlaylistConfigurationContainer).attr('data-action-url'), function () {
                                $(containers.TelevisionConfigurationContainer).load($(containers.TelevisionConfigurationContainer).attr('data-action-url'), callback);
                            });
                        });
                    });
                });
            });
        });
    }
}

