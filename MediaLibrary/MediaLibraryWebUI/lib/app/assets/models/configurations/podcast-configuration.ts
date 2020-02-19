import BaseConfiguration from './base-configuration';
import IPodcastConfiguration from '../../interfaces/podcast-configuration-interface';

export default class PodcastConfiguration extends BaseConfiguration {
    constructor(public readonly properties: IPodcastConfiguration) {
        super('Podcast');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IPodcastConfiguration>(this.properties, callback);
    }
}
