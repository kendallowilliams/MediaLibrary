import BaseConfiguration from './base-configuration';
import IPlayerConfiguration from '../../interfaces/player-configuration-interface';

export default class PlayerConfiguration extends BaseConfiguration {
    constructor(private properties: IPlayerConfiguration) {
        super('Player');
    }

    updateConfiguration(callback: () => void = () => null): void {
        super.update<IPlayerConfiguration>(this.properties, callback);
    }
}
