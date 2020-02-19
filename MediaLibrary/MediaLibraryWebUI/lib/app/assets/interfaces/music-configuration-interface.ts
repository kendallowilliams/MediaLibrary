import { AlbumSort, ArtistSort, SongSort, MusicTabs, MusicPages } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IMusicConfiguration extends IConfiguration {
    selectedAlbumId: number;
    selectedArtistId: number;
    selectedAlbumSort: AlbumSort;
    selectedArtistSort: ArtistSort;
    selectedSongSort: SongSort;
    selectedMusicTab: MusicTabs;
    selectedMusicPage: MusicPages;
}