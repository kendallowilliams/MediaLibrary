import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import HomeConfiguration from "../../assets/models/configurations/home-configuration";

export default class Home extends BaseClass implements IView {
    constructor(private homeConfiguration: HomeConfiguration) {
        super();
    }

    loadView(): void {
        throw new Error("Method not implemented.");
    }
}