import BaseConfiguration from './base-configuration';
import ITelevisionConfiguration from '../../interfaces/television-configuration-interface';

export default class TelevisionConfiguration extends BaseConfiguration {
    constructor(public readonly properties: ITelevisionConfiguration) {
        super('Television');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<ITelevisionConfiguration>(this.properties, callback);
    }
}
