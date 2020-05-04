import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlaylistConfiguration from "../../assets/models/configurations/playlist-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PlaylistPages } from "../../assets/enums/enums";
import AddNewPlaylistModal from "../../assets/modals/add-playlist-modal";
import LoadingModal from "../../assets/modals/loading-modal";
import EditPlaylistModal from "../../assets/modals/edit-playlist-modal";
import { loadTooltips, disposeTooltips } from "../../assets/utilities/bootstrap-helper";
import { getPlaylistSortEnum, getPlaylistTabEnumString, getPlaylistTabEnum, getPlaylistSortEnumString } from "../../assets/enums/enum-functions";
import DownloadM3UPlaylistModal from "../../assets/modals/download-m3u-playlist-modal";
import IPlayerLoadFunctions from "../../assets/interfaces/player-load-functions-interface";

export default class Playlist extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private addPlaylistModal: AddNewPlaylistModal;
    private editPlaylistModal: EditPlaylistModal;
    private downloadM3UPlaylistModal: DownloadM3UPlaylistModal;
    private playlistView: HTMLElement;

    constructor(private playlistConfiguration: PlaylistConfiguration,
        private playFunc: (btn: HTMLButtonElement) => void,
        private updateActiveMediaFunc: () => void,
        private loadFunctions: IPlayerLoadFunctions) {
        super();
        this.playlistView = HtmlControls.Views().MediaView;
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const success: () => void = () => {
            this.addPlaylistModal = new AddNewPlaylistModal(this.loadView.bind(this));
            this.editPlaylistModal = new EditPlaylistModal(this.loadView.bind(this));
            this.downloadM3UPlaylistModal = new DownloadM3UPlaylistModal();
            this.initializeControls();
            this.updateActiveMediaFunc();
            this.applyLoadFunctions();
            $('[data-playlist-tab="' + getPlaylistTabEnumString(this.playlistConfiguration.properties.SelectedPlaylistTab) + '"]').tab('show');
            callback();
        };

        disposeTooltips(this.mediaView);
        $(this.mediaView).load('Playlist/Index', success);
    }

    private initializeControls(): void {
        loadTooltips(this.mediaView);
        $(this.mediaView).find('*[data-back-button="playlist"]').on('click', () => this.goBack(() => this.loadView.call(this)));
        $(this.mediaView).find('*[data-play-id]').on('click', e => this.playFunc(e.currentTarget as HTMLButtonElement));

        $(this.mediaView).find('*[data-playlist-action="sort"]').on('change', e => {
            const $target = $(e.currentTarget),
                sortType = $target.attr('data-sort-type');

            LoadingModal.showLoading();
            if (sortType === 'SelectedMusicPlaylistSort') {
                this.playlistConfiguration.properties.SelectedMusicPlaylistSort = getPlaylistSortEnum($target.val() as string);
            } else if (sortType === 'SelectedPodcastPlaylistSort') {
                this.playlistConfiguration.properties.SelectedPodcastPlaylistSort = getPlaylistSortEnum($target.val() as string);
            }else if (sortType === 'SelectedTelevisionPlaylistSort') {
                this.playlistConfiguration.properties.SelectedTelevisionPlaylistSort = getPlaylistSortEnum($target.val() as string);
            }
            this.playlistConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('*[data-playlist-id]').on('click', e => {
            LoadingModal.showLoading();
            this.playlistConfiguration.properties.SelectedPlaylistId = parseInt($(e.currentTarget).attr('data-playlist-id'));
            this.playlistConfiguration.properties.SelectedPlaylistPage = PlaylistPages.Playlist;
            this.playlistConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(HtmlControls.UIControls().PlaylistTabList).find('*[data-toggle="tab"]').on('shown.bs.tab', e => {
            const $newTab = $(e.target),
                $oldTab = $(e.relatedTarget);

            this.playlistConfiguration.properties.SelectedPlaylistTab = getPlaylistTabEnum($newTab.attr('data-playlist-tab'));
            this.playlistConfiguration.updateConfiguration();

            $(HtmlControls.UIControls().PlaylistTabList).find('*[data-sort-tab]').each((index, _btn) => {
                if ($(_btn).attr('data-sort-tab') === $newTab.attr('id')) {
                    $(_btn).addClass('d-inline-block').removeClass('d-none');
                } else {
                    $(_btn).removeClass('d-inline-block').addClass('d-none');
                }
            });
        });
    }

    private goBack(callback: () => void = () => null): void {
        this.playlistConfiguration.properties.SelectedPlaylistId = 0;
        this.playlistConfiguration.properties.SelectedPlaylistPage = PlaylistPages.Index;
        this.playlistConfiguration.updateConfiguration(callback);
    }

    private applyLoadFunctions(): void {
        $(this.playlistView).find('*[data-artist-id]').on('click', e => this.loadFunctions.loadArtist(parseInt($(e.currentTarget).attr('data-artist-id'))));
        $(this.playlistView).find('*[data-album-id]').on('click', e => this.loadFunctions.loadAlbum(parseInt($(e.currentTarget).attr('data-album-id'))));
        $(this.playlistView).find('*[data-podcast-id]').on('click', e => this.loadFunctions.loadPodcast(parseInt($(e.currentTarget).attr('data-podcast-id'))));
        $(this.playlistView).find('*[data-series-id]').on('click', e => this.loadFunctions.loadSeries(parseInt($(e.currentTarget).attr('data-series-id'))));
    }
}