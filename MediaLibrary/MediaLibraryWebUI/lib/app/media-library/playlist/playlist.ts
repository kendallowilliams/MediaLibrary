import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlaylistConfiguration from "../../assets/models/configurations/playlist-configuration";

export default class Playlist extends BaseClass implements IView {
    constructor(private playlistConfiguration: PlaylistConfiguration) {
        super();
    }

    loadView(): void {

    }
}