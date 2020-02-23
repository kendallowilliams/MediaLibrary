import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import TelevisionConfiguration from "../../assets/models/configurations/television-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { TelevisionPages, SeriesSort } from "../../assets/enums/enums";
import ITelevisionConfiguration from "../../assets/interfaces/television-configuration-interface";
import LoadingModal from '../../assets/modals/loading-modal';
import { loadTooltips, disposeTooltips } from "../../assets/utilities/bootstrap-helper";

export default class Television extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private seasonView: HTMLElement;

    constructor(private televisionConfiguration: TelevisionConfiguration, private playFunc: (btn: HTMLButtonElement) => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const properties: ITelevisionConfiguration = this.televisionConfiguration.properties,
            success: () => void = () => {
                this.seasonView = HtmlControls.Views().SeasonView;
                this.initializeControls();
                $('[data-season-id][data-item-index="0"]').trigger('click');
                callback();
            };

        $(this.mediaView).load('/Television/Index', success);
    }

    initializeControls(): void {
        $('[data-play-id]').on('click', e => this.playFunc(e.target as HTMLButtonElement));
        $('[data-back-button="television"]').on('click', () => this.goBack(() => this.loadView.call(this)));

        $(this.mediaView).find('*[data-series-action="sort"]').on('click', e => {
            LoadingModal.showLoading();
            this.televisionConfiguration.properties.SelectedSeriesSort = this.getSeriesSortEnum($(e.currentTarget).val() as string);
            this.televisionConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('*[data-series-id]').on('click', e => {
            LoadingModal.showLoading();
            this.televisionConfiguration.properties.SelectedSeriesId = parseInt($(e.currentTarget).attr('data-series-id'));
            this.televisionConfiguration.properties.SelectedTelevisionPage = TelevisionPages.Series;
            this.televisionConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('*[data-series-action="playlist"]').on('click', e => {
            const season = this.televisionConfiguration.properties.SelectedSeason,
                series = this.televisionConfiguration.properties.SelectedSeriesId;

            window.location.href = '/Television/GetM3UPlaylist?seriesId=' + series + '&season=' + season;
        });

        $(this.mediaView).find('*[data-season-id]').on('click', e => {
            const item = e.currentTarget,
                success = () => {
                    $(item).parent('li.page-item:first').addClass('active');
                    this.updateMobileSeasons(parseInt(id));
                    loadTooltips(this.seasonView);
                    LoadingModal.hideLoading();
                },
                series = this.televisionConfiguration.properties.SelectedSeriesId,
                id = $(item).attr('data-season-id'),
                selectedSeason = this.televisionConfiguration.properties.SelectedSeason;

            if (id === '-' && (selectedSeason - 1) > 0) {
                $(this.mediaView).find('[data-season-id="' + (selectedSeason - 1) + '"]').trigger('click');
            } else if (id === '+' && (selectedSeason + 1) > 0) {
                $(this.mediaView).find('[data-season-id="' + (selectedSeason + 1) + '"]').trigger('click');
            } else if (parseInt(id) > 0) {
                $(this.mediaView).find('li.page-item').removeClass('active');
                LoadingModal.showLoading();
                this.televisionConfiguration.properties.SelectedSeason = parseInt(id);
                disposeTooltips(this.seasonView);
                $(this.seasonView).load('/Television/GetSeason', { series: series, season: parseInt(id) }, success);
            }
        });
    }

    private updateMobileSeasons(season: number): void {
        let minSeasonCount = 5,
            numItemsBefore = Math.floor(minSeasonCount / 2),
            numItemsAfter = minSeasonCount - numItemsBefore - 1,
            first = season - numItemsBefore,
            last = season + numItemsAfter,
            cssSelector = '[data-season-id]:not([data-season-id="+"]):not([data-season-id="-"]',
            numSeasons = $(cssSelector).length;

        $(cssSelector).addClass('d-none d-lg-block');

        if (first < 1) {
            first = 1;
            last = minSeasonCount;
        } else if (last > numSeasons) {
            first = first - (last - numSeasons);
            last = season + (numSeasons - season);
        }

        for (var i = first; i <= last; i++) {
            $(this.mediaView).find('*[data-season-id="' + i + '"]').removeClass('d-none d-lg-block');
        }
    }

    private goBack(callback: () => void = () => null): void {
        this.televisionConfiguration.properties.SelectedSeriesId = 0;
        this.televisionConfiguration.properties.SelectedTelevisionPage = TelevisionPages.Index;
        this.televisionConfiguration.updateConfiguration(callback);
    }

    private getSeriesSortEnum(sort: string): SeriesSort {
        let seriesSort: SeriesSort;

        switch (sort) {
            case 'AtoZ':
            default:
                seriesSort = SeriesSort.AtoZ;
                break;
        }

        return seriesSort;
    }
}