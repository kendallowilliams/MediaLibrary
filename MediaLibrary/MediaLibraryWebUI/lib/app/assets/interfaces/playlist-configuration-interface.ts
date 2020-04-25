﻿import { PlaylistPages, PlaylistSort, PlaylistTabs } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IPlaylistConfiguration extends IConfiguration {
    SelectedPlaylistId: number;
    SelectedPlaylistPage: PlaylistPages;
    SelectedPlaylistSort: PlaylistSort;
    SelectedPlaylistTab: PlaylistTabs;
}