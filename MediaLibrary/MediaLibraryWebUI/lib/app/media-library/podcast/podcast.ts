import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PodcastConfiguration from "../../assets/models/configurations/podcast-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PodcastPages, PodcastSort, PodcastFilter } from "../../assets/enums/enums";
import IPodcastConfiguration from "../../assets/interfaces/podcast-configuration-interface";
import AddNewPodcastModal from "../../assets/modals/add-podcast-modal";
import LoadingModal from "../../assets/modals/loading-modal";
import DeleteModal from "../../assets/modals/delete-modal";
import { disposeTooltips, loadTooltips } from "../../assets/utilities/bootstrap-helper";

export default class Podcast extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private podcastView: HTMLElement;
    private addNewPodcastModal: AddNewPodcastModal;
    private deleteModal: DeleteModal;

    constructor(private podcastConfiguration: PodcastConfiguration, private playFunc: (btn: HTMLButtonElement) => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const properties: IPodcastConfiguration = this.podcastConfiguration.properties,
            success: () => void = () => {
                this.podcastView = HtmlControls.Views().PodcastView;
                this.addNewPodcastModal = new AddNewPodcastModal(this.loadView.bind(this));
                this.deleteModal = new DeleteModal(this.loadView.bind(this));
                this.initializeControls();
                $('[data-podcast-year][data-item-index="0"]').trigger('click');
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


        $(this.mediaView).find('*[data-podcast-year]').on('click', e => {
            var year = $(e.currentTarget).attr('data-podcast-year'),
                years = $(this.podcastView).attr('data-podcast-years').split(','),
                currentIndex = years.indexOf(this.getSelectedYear());

            if (year === '-' && currentIndex > 0) {
                $('[data-podcast-year="' + years[currentIndex - 1] + '"]').trigger('click');
            } else if (year === '+' && (currentIndex + 1) < years.length) {
                $('[data-podcast-year="' + years[currentIndex + 1] + '"]').trigger('click');
            } else if (parseInt(year) > 0) {
                $('li.page-item').removeClass('active');
                this.loadPodcastView(e.currentTarget);
            }
        });

        $(this.mediaView).find('*[data-podcast-action="filter"]').on('change', e => {
            LoadingModal.showLoading();
            this.podcastConfiguration.properties.SelectedPodcastFilter = this.getPodcastFilterEnum($(e.currentTarget).val() as string);
            this.podcastConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('*[data-podcast-action="refresh"]').on('click', e => {
            LoadingModal.showLoading();
            $.post('/Podcast/RefreshPodcast', { id: this.podcastConfiguration.properties.SelectedPodcastId }, () => this.loadView(() => LoadingModal.hideLoading()));
        });
    }

    private getSelectedYear(): string {
        return $('li.page-item.active a.page-link[data-podcast-year]').attr('data-podcast-year');
    }

    private loadPodcastView(item): void {
        var success = () => {
            $(item).parent('li.page-item:first').addClass('active');
            loadTooltips(this.podcastView);
            LoadingModal.hideLoading();
        },
            id = this.podcastConfiguration.properties.SelectedPodcastId,
            year = $(item).attr('data-podcast-year'),
            filter = this.podcastConfiguration.properties.SelectedPodcastFilter;

        if (item) {
            LoadingModal.showLoading();
            disposeTooltips(this.podcastView);
            $(this.podcastView).load('/Podcast/GetPodcastItems', { id: id, year: year, filter: filter }, success);
        }
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

    private getPodcastFilterEnum(filter: string): PodcastFilter {
        let podcastFilter: PodcastFilter;

        switch (filter) {
            case 'Downloaded':
                podcastFilter = PodcastFilter.Downloaded;
                break;
            case 'Unplayed':
                podcastFilter = PodcastFilter.Unplayed;
                break;
            case 'All':
            default:
                podcastFilter = PodcastFilter.All;
                break;
        }

        return podcastFilter;
    }
}