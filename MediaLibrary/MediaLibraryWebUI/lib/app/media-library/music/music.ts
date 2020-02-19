import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";

export default class Music extends BaseClass implements IView {
    constructor(private musicConfiguration: MusicConfiguration) {
        super();
    }

    loadView(): void {
        switch (this.musicConfiguration.properties.selectedMusicPage) {
            case MusicPages.Album:
                this.loadAlbum(this.musicConfiguration.properties.selectedAlbumId);
                break;
            case MusicPages.Artist:
                this.loadArtist(this.musicConfiguration.properties.selectedArtistId);
                break;
            case MusicPages.Index:
            default:
                this.loadIndex();
                break;
        }
    }

    loadIndex(): void {

    } 

    loadArtist(id: number): void {

    }

    loadAlbum(id: number): void {

    }
}