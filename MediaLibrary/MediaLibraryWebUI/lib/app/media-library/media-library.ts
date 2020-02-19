import Music from './music/music';
import Player from './player/player';
import Playlist from './playlist/playlist';
import Television from './television/television';
import Podcast from './podcast/podcast';
import HtmlControls from '../assets/html-controls';
import Configurations from '../assets/configurations';
import IBaseClass from '../assets/interfaces/class-interface'

export default class MediaLibrary implements IBaseClass {
    $: JQuery<HTMLElement>;
    private music: Music;
    private player: Player;
    private playlist: Playlist;
    private television: Television;
    private podcast: Podcast;

    constructor() {
        this.music = new Music();
        this.player = new Player();
        this.playlist = new Playlist();
        this.podcast = new Podcast();
        this.television = new Television();

        this.loadConfigurations();
    }

    loadConfigurations(): void {
        $(HtmlControls.Containers.HomeConfigurationContainer).load($(HtmlControls.Containers.HomeConfigurationContainer).attr('data-action-url'));
    }
}

