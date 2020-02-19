import { PlaylistPages, PlaylistSort } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IPlaylistConfiguration extends IConfiguration {
    SelectedPlaylistId: number;
    SelectedPlaylistPage: PlaylistPages;
    SelectedPlaylistSort: PlaylistSort;
}