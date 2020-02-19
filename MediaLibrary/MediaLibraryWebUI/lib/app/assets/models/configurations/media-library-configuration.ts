import BaseConfiguration from './base-configuration';
import IMediaLibraryConfiguration from '../../interfaces/media-library-configuration-interface';

export default class MediaLibraryConfiguration extends BaseConfiguration {
    constructor(private properties: IMediaLibraryConfiguration) {
        super('MediaLibrary');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IMediaLibraryConfiguration>(this.properties, callback);
    }
}
