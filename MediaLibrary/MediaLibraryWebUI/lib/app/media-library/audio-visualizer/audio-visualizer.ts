import BaseClass from "../../assets/models/base-class";
import PlayerConfiguration from "../../assets/models/configurations/player-configuration";

export default class AudioVisualizer extends BaseClass {

    constructor(private playerConfiguration: PlayerConfiguration) {
        super();
    }
}