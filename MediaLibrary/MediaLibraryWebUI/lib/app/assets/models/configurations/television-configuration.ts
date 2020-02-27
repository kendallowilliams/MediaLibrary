import BaseConfiguration from './base-configuration';
import ITelevisionConfiguration from '../../interfaces/television-configuration-interface';

export default class TelevisionConfiguration extends BaseConfiguration<ITelevisionConfiguration> {
    constructor(properties: ITelevisionConfiguration) {
        super('Television');
        this.properties = properties;
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<ITelevisionConfiguration>(this.properties, callback);
    }
}
