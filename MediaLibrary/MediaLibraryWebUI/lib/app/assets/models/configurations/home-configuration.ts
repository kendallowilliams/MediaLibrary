import BaseConfiguration from './base-configuration';
import IHomeConfiguration from '../../interfaces/home-configuration-interface';

export default class HomeConfiguration extends BaseConfiguration {
    constructor(public readonly properties: IHomeConfiguration) {
        super('Home');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IHomeConfiguration>(this.properties, callback);
    }
}
