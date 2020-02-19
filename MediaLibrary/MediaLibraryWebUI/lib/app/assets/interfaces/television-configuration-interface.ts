import { AlbumSort, ArtistSort, SongSort, MusicTabs, MusicPages, TelevisionPages, SeriesSort } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface ITelevisionConfiguration extends IConfiguration {
    selectedSeriesId: number;
    selectedSeason: number;
    selectedTelevisionPage: TelevisionPages;
    selectedSeriesSort: SeriesSort;
}