import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlaylistConfiguration from "../../assets/models/configurations/playlist-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { PlaylistPages } from "../../assets/enums/enums";
import AddNewPlaylistModal from "../../assets/modals/add-playlist-modal";
import LoadingModal from "../../assets/modals/loading-modal";
import EditPlaylistModal from "../../assets/modals/edit-playlist-modal";
import { loadTooltips, disposeTooltips } from "../../assets/utilities/bootstrap-helper";
import { getPlaylistSortEnum } from "../../assets/enums/enum-functions";

export default class Playlist extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private addPlaylistModal: AddNewPlaylistModal;
    private editPlaylistModal: EditPlaylistModal;

    constructor(private playlistConfiguration: PlaylistConfiguration,
        private playFunc: (btn: HTMLButtonElement) => void,
        private updateActiveMediaFunc: () => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const success: () => void = () => {
            this.addPlaylistModal = new AddNewPlaylistModal(this.loadView.bind(this));
            this.editPlaylistModal = new EditPlaylistModal(this.loadView.bind(this));
            this.initializeControls();
            this.updateActiveMediaFunc();
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
            LoadingModal.showLoading();
            this.playlistConfiguration.properties.SelectedPlaylistSort = getPlaylistSortEnum($(e.currentTarget).val() as string);
            this.playlistConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });

        $(this.mediaView).find('*[data-playlist-id]').on('click', e => {
            LoadingModal.showLoading();
            this.playlistConfiguration.properties.SelectedPlaylistId = parseInt($(e.currentTarget).attr('data-playlist-id'));
            this.playlistConfiguration.properties.SelectedPlaylistPage = PlaylistPages.Playlist;
            this.playlistConfiguration.updateConfiguration(() => this.loadView(() => LoadingModal.hideLoading()));
        });
    }

    private goBack(callback: () => void = () => null): void {
        this.playlistConfiguration.properties.SelectedPlaylistId = 0;
        this.playlistConfiguration.properties.SelectedPlaylistPage = PlaylistPages.Index;
        this.playlistConfiguration.updateConfiguration(callback);
    }
}