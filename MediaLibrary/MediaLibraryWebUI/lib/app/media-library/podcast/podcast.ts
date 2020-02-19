import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PodcastConfiguration from "../../assets/models/configurations/podcast-configuration";

export default class Podcast extends BaseClass implements IView {
    constructor(private podcastConfiguration: PodcastConfiguration) {
        super();
    }

    loadView(): void {
    }
}