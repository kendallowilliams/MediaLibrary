import BaseConfiguration from './base-configuration';
import IHomeConfiguration from '../../interfaces/home-configuration-interface';

export default class HomeConfiguration extends BaseConfiguration<IHomeConfiguration> {
    constructor(properties: IHomeConfiguration) {
        super('Home');
        this.properties = properties;
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IHomeConfiguration>(this.properties, callback);
    }
}
