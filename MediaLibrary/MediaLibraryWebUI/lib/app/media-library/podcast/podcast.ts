import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PodcastConfiguration from "../../assets/models/configurations/podcast-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PodcastPages } from "../../assets/enums/enums";

export default class Podcast extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;

    constructor(private podcastConfiguration: PodcastConfiguration) {
        super();
        this.mediaView = HtmlControls.Views.MediaView;
    }

    loadView(): void {
        $(this.mediaView).load('/Podcast/Index', () => {
            this.initializeControls();
        });
    }

    initializeControls(): void {
        $('[data-back-button="podcast"]').on('click', () => this.goBack(() => this.loadView.call(this)));
    }

    loadPodcast(id: number, callback: () => void = () => null): void {
        this.podcastConfiguration.properties.SelectedPodcastId = id;
        this.podcastConfiguration.properties.SelectedPodcastPage = PodcastPages.Podcast;
        this.podcastConfiguration.updateConfiguration(callback);
    }

    goBack(callback: () => void = () => null): void {
        this.podcastConfiguration.properties.SelectedPodcastId = 0;
        this.podcastConfiguration.properties.SelectedPodcastPage = PodcastPages.Index;
        this.podcastConfiguration.updateConfiguration(callback);
    }
}