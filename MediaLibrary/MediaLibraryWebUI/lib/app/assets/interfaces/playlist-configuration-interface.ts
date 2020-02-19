import { PlaylistPages, PlaylistSort } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IPlaylistConfiguration extends IConfiguration {
    selectedPlaylistId: number;
    selectedPlaylistPage: PlaylistPages;
    selectedPlaylistSort: PlaylistSort;
}