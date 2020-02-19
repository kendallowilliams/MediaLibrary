import BaseConfiguration from './base-configuration'
import * as Enums from '../assets/enums'

export default class PlaylistConfiguration extends BaseConfiguration {
    private selectedPlaylistId: number;
    private selectedPlaylistPage: Enums.PlaylistPages;
    private selectedPlaylistSort: Enums.PlaylistSort;

    constructor() {
        super();
    }
}
