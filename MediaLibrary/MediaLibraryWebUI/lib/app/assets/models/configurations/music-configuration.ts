import BaseConfiguration from './base-configuration';
import IMusicConfiguration from '../../interfaces/music-configuration-interface';

export default class MusicConfiguration extends BaseConfiguration {
    constructor(private properties: IMusicConfiguration) {
        super('Music');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IMusicConfiguration>(this.properties, callback);
    }
}
