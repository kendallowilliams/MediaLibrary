import BaseConfiguration from './base-configuration'
import * as Enums from '../assets/enums'

export default class MusicConfiguration extends BaseConfiguration {
    private selectedAlbumId: number;
    private selectedArtistId: number;
    private selectedAlbumSort: Enums.AlbumSort;
    private selectedArtistSort: Enums.ArtistSort;
    private selectedSongSort: Enums.SongSort;
    private selectedMusicTab: Enums.MusicTabs;
    private selectedMusicPage: Enums.MusicPages;

    constructor() {
        super();
    }
}
