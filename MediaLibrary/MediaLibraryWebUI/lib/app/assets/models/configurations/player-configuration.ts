import BaseConfiguration from './base-configuration'
import * as Enums from '../../enums/enums'

export default class PlayerConfiguration extends BaseConfiguration {
    private selectedMediaType: Enums.MediaTypes;
    private currentItemIndex: number;
    private autoPlay: boolean;
    private repeat: Enums.RepeatTypes;
    private shuffle: boolean;
    private selectedPlayerPage: Enums.PlayerPages;
    private volume: number;
    private muted: boolean;
    private audioVisualizerEnabled: boolean;

    constructor(json: any) {
        super();
    }
}
