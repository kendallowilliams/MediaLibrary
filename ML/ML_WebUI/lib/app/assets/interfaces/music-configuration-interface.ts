import { AlbumSort, ArtistSort, SongSort, MusicTabs, MusicPages } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IMusicConfiguration extends IConfiguration {
    SelectedAlbumId: number;
    SelectedArtistId: number;
    SelectedAlbumSort: AlbumSort;
    SelectedArtistSort: ArtistSort;
    SelectedSongSort: SongSort;
    SelectedMusicTab: MusicTabs;
    SelectedMusicPage: MusicPages;
    PreviousSearchQuery: string;
}