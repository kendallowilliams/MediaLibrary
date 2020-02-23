import htmlControls from "../controls/html-controls";
import LoadingModal from "./loading-modal";

export default class AddNewPlaylistModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = htmlControls.Modals().NewPlaylistModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', function (e) {
            $('#txtNewPlaylist').val('');
        });

        $(this.modal).find('*[data-playlist-action="save"]').on('click', e => {
            LoadingModal.showLoading();
            $(this.modal).modal('hide').on('hidden.bs.modal', () => {
                $.post('Playlist/AddPlaylist', { playlistName: $('#txtNewPlaylist').val() }, () => this.loadFunc(() => LoadingModal.hideLoading()));
            });
        });
    }
}