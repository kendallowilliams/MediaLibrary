import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlayerConfiguration from "../../assets/models/configurations/player-configuration";

export default class Player extends BaseClass implements IView {
    constructor(private playerConfiguration: PlayerConfiguration) {
        super();
    }

    loadView(): void {

    }
}