import BaseConfiguration from './base-configuration';
import IPlaylistConfiguration from '../../interfaces/playlist-configuration-interface';

export default class PlaylistConfiguration extends BaseConfiguration {
    constructor(public readonly properties: IPlaylistConfiguration) {
        super('Playlist');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IPlaylistConfiguration>(this.properties, callback);
    }
}
