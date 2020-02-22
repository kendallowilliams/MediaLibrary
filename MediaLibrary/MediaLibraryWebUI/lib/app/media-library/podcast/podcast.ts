import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PodcastConfiguration from "../../assets/models/configurations/podcast-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PodcastPages } from "../../assets/enums/enums";
import IPodcastConfiguration from "../../assets/interfaces/podcast-configuration-interface";
import AddNewPodcastModal from "../../assets/modals/add-podcast-modal";

export default class Podcast extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private addNewPodcastModal: AddNewPodcastModal;

    constructor(private podcastConfiguration: PodcastConfiguration, private playFunc: (btn: HTMLButtonElement) => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const properties: IPodcastConfiguration = this.podcastConfiguration.properties,
            success: () => void = () => {
                this.addNewPodcastModal = new AddNewPodcastModal(this.loadView.bind(this));
                this.initializeControls();
                callback();
            };

        $(this.mediaView).load('/Podcast/Index', success);
    }

    initializeControls(): void {
        $('[data-play-id]').on('click', e => this.playFunc(e.target as HTMLButtonElement));
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