import BaseConfiguration from './base-configuration';
import IPlaylistConfiguration from '../../interfaces/playlist-configuration-interface';

export default class PlaylistConfiguration extends BaseConfiguration<IPlaylistConfiguration> {
    constructor(properties: IPlaylistConfiguration) {
        super('Playlist');
        this.properties = properties;
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IPlaylistConfiguration>(this.properties, callback);
    }
}
