import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlaylistConfiguration from "../../assets/models/configurations/playlist-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PlaylistPages } from "../../assets/enums/enums";

export default class Playlist extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;

    constructor(private playlistConfiguration: PlaylistConfiguration) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const success: () => void = () => {
            this.initializeControls();
            callback();
        };

        $(this.mediaView).load('/Playlist/Index', success);
    }

    initializeControls(): void {
        $('[data-back-button="playlist"]').on('click', () => this.goBack(() => this.loadView.call(this)));
    }

    loadPlaylist(id: number, callback: () => void = () => null): void {
        this.playlistConfiguration.properties.SelectedPlaylistId = id;
        this.playlistConfiguration.properties.SelectedPlaylistPage = PlaylistPages.Playlist;
        this.playlistConfiguration.updateConfiguration(callback);
    }

    goBack(callback: () => void = () => null): void {
        this.playlistConfiguration.properties.SelectedPlaylistId = 0;
        this.playlistConfiguration.properties.SelectedPlaylistPage = PlaylistPages.Index;
        this.playlistConfiguration.updateConfiguration(callback);
    }
}