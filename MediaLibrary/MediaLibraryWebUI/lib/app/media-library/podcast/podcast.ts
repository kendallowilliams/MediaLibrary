import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PodcastConfiguration from "../../assets/models/configurations/podcast-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PodcastPages, PodcastSort, PodcastFilter } from "../../assets/enums/enums";
import IPodcastConfiguration from "../../assets/interfaces/podcast-configuration-interface";
import AddNewPodcastModal from "../../assets/modals/add-podcast-modal";
import LoadingModal from "../../assets/modals/loading-modal";
import { disposeTooltips, loadTooltips } from "../../assets/utilities/bootstrap-helper";

export default class Podcast extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private podcastView: HTMLElement;
    private addNewPodcastModal: AddNewPodcastModal;

    constructor(private podcastConfiguration: PodcastConfiguration,
        private playFunc: (btn: HTMLButtonElement, single: boolean) => void,
        private updateActiveMediaFunc: () => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const properties: IPodcastConfiguration = this.podcastConfiguration.properties,
            success: () => void = () => {
                this.podcastView = HtmlControls.Views().PodcastView;
                this.addNewPodcastModal = new AddNewPodcastModal(this.loadView.bind(this));
                this.initializeControls();
                $('[data-podcast-year][data-item-index="1"]').trigger('click');
                callback();
            };
        
        disposeTooltips(this.mediaView);
        this.podcastConfiguration.refresh(() => $(this.mediaView).load('Podcast/Index', success));
    }

    initializeControls(): void {
        loadTooltips(this.mediaView);
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

        $(this.mediaView).find('*[data-podcast-id]').on('click', e => this.loadPodcast(parseInt($(e.currentTarget).attr('data-podcast-id')), () => this.loadView()));
        
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
            $.post('Podcast/RefreshPodcast', { id: this.podcastConfiguration.properties.SelectedPodcastId }, () => this.loadView(() => LoadingModal.hideLoading()));
        });
    }

    loadPodcast(id: number, callback: () => void = () => null) {
        if (Number.isInteger(id)) {
            this.podcastConfiguration.properties.SelectedPodcastPage = PodcastPages.Podcast;
            this.podcastConfiguration.properties.SelectedPodcastId = id;
            this.podcastConfiguration.updateConfiguration(callback);
        }
    }

    private getSelectedYear(): string {
        return $('li.page-item.active a.page-link[data-podcast-year]').attr('data-podcast-year');
    }

    private loadPodcastView(item): void {
        var success = () => {
            $(item).parent('li.page-item:first').addClass('active');
            this.updateMobileYears(parseInt($(item).attr('data-item-index')));
            loadTooltips(this.podcastView);
            $(this.mediaView).find('*[data-play-id]').on('click', e => this.playFunc(e.currentTarget as HTMLButtonElement, true));
            $(this.mediaView).find('*[data-podcast-action="download"]').on('click', e => {
                const $btn = $(e.currentTarget);

                LoadingModal.showLoading();
                $btn.tooltip('dispose');
                $btn.prop('disabled', 'disabled');
                $.get($btn.attr('data-download-action'), () => LoadingModal.hideLoading());
            });
            this.updateActiveMediaFunc();
            LoadingModal.hideLoading();
        },
            id = this.podcastConfiguration.properties.SelectedPodcastId,
            year = $(item).attr('data-podcast-year'),
            filter = this.podcastConfiguration.properties.SelectedPodcastFilter;

        if (item) {
            LoadingModal.showLoading();
            disposeTooltips(this.podcastView);
            $(this.podcastView).load('Podcast/GetPodcastItems', { id: id, year: year, filter: filter }, success);
        }
    }

    private updateMobileYears(position: number): void {
        let minYearCount = 5,
            maxYearCount = 10,
            numItemsBefore = Math.ceil(minYearCount / 2) - 1,
            numItemsAfter = minYearCount - numItemsBefore - 1,
            first = position - numItemsBefore,
            last = position + numItemsAfter,
            cssSelector = '[data-podcast-year]:not([data-podcast-year="+"]):not([data-podcast-year="-"]',
            numYears = $(cssSelector).length,
            delta = numYears - maxYearCount;

        $(cssSelector).addClass('d-none d-lg-block');

        if (first < 1) {
            first = 1;
            last = minYearCount;
        } else if (last > numYears) {
            first = first - (last - numYears);
            last = numYears;
        }

        if (delta > 0) {
            let maxFirst: number = position - (Math.ceil(maxYearCount / 2) - 1),
                maxLast: number = position + Math.ceil(maxYearCount / 2);

            if (maxFirst < 1) {
                maxFirst = 1;
                maxLast = maxYearCount;
            } else if (maxLast > numYears) {
                maxFirst = maxFirst - (maxLast - numYears);
                maxLast = numYears;
            }

            for (let i = 1; i <= numYears; i++) {
                if (i < maxFirst || i > maxLast) {
                    $(this.mediaView).find('*[data-podcast-year][data-item-index="' + i + '"]').removeClass('d-lg-block');
                }
            }
        }

        for (let i = first; i <= last; i++) {
            $(this.mediaView).find('*[data-podcast-year][data-item-index="' + i + '"]').removeClass('d-none d-lg-block');
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