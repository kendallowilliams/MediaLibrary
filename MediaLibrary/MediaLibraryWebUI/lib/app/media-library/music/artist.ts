import BaseClass from "../../assets/models/base-class";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";

export default class Artist extends BaseClass {
    constructor(private musicConfiguration: MusicConfiguration) {
        super();
    }

    initializeControls(): void {
        $('[data-back-button="artist"]').on('click', () => this.goBack());
    }

    loadArtist(id: number, callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedArtistId = id;
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Artist;
        this.musicConfiguration.updateConfiguration(callback);
    }

    goBack(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedArtistId = 0;
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Index;
        this.musicConfiguration.updateConfiguration(callback);
    }
}