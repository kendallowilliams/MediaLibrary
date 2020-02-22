import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PodcastConfiguration from "../../assets/models/configurations/podcast-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PodcastPages, PodcastSort } from "../../assets/enums/enums";
import IPodcastConfiguration from "../../assets/interfaces/podcast-configuration-interface";
import AddNewPodcastModal from "../../assets/modals/add-podcast-modal";
import LoadingModal from "../../assets/modals/loading-modal";
import DeleteModal from "../../assets/modals/delete-modal";

export default class Podcast extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private addNewPodcastModal: AddNewPodcastModal;
    private deleteModal: DeleteModal;

    constructor(private podcastConfiguration: PodcastConfiguration, private playFunc: (btn: HTMLButtonElement) => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const properties: IPodcastConfiguration = this.podcastConfiguration.properties,
            success: () => void = () => {
                this.addNewPodcastModal = new AddNewPodcastModal(this.loadView.bind(this));
                this.deleteModal = new DeleteModal(this.loadView.bind(this));
                this.initializeControls();
                callback();
            };

        $(this.mediaView).load('/Podcast/Index', success);
    }

    initializeControls(): void {
        $('[data-play-id]').on('click', e => this.playFunc(e.target as HTMLButtonElement));
        $('[data-back-button="podcast"]').on('click', () => {
            LoadingModal.showLoading();
            this.podcastConfiguration.properties.SelectedPodcastId = 0;
            this.podcastConfiguration.properties.SelectedPodcastPage = PodcastPages.Index;
            this.podcastConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('[data-podcast-action="sort"]').on('change', e => {
            LoadingModal.showLoading();
            this.podcastConfiguration.properties.SelectedPodcastSort = this.getPodcastSortEnum($(e.currentTarget).val() as string);
            this.podcastConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('*[data-podcast-id]').on('click', e => {
            LoadingModal.showLoading();
            this.podcastConfiguration.properties.SelectedPodcastPage = PodcastPages.Podcast;
            this.podcastConfiguration.properties.SelectedPodcastId = parseInt($(e.currentTarget).attr('data-podcast-id'));
            this.podcastConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });
    }

    private getPodcastSortEnum(sort: string): PodcastSort {
        let podcastSort: PodcastSort;

        switch (sort) {
            case 'LastUpdateDate':
                podcastSort = PodcastSort.LastUpdateDate;
                break;
            case 'DateAdded':
                podcastSort = PodcastSort.DateAdded;
                break;
            case 'AtoZ':
            default:
                podcastSort = PodcastSort.AtoZ;
                break;
        }

        return podcastSort;
    }
}