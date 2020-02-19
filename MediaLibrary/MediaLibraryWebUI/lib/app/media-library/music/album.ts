import BaseClass from "../../assets/models/base-class";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";

export default class Album extends BaseClass {
    constructor(private musicConfiguration: MusicConfiguration) {
        super();
    }

    loadAlbum(id: number, callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedAlbumId = id;
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Album;
        this.musicConfiguration.updateConfiguration(callback);
    }

    goBack(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedAlbumId = 0;
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Index;
        this.musicConfiguration.updateConfiguration(callback);
    }
}