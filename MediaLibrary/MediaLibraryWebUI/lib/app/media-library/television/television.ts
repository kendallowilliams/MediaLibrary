import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import TelevisionConfiguration from "../../assets/models/configurations/television-configuration";

export default class Television extends BaseClass implements IView {
    constructor(private televisionConfiguration: TelevisionConfiguration) {
        super();
    }

    loadView(): void {

    }
}