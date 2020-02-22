import HtmlControls from "../controls/html-controls";
import LoadingModal from "./loading-modal";

export default class EditPlaylistModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().EdiPlaylistModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', function (e) {
            $('#txtPlaylistId').val($(e.relatedTarget).attr('data-item-id'));
            $('#txtPlaylistName').val($(e.relatedTarget).attr('data-item-name'));
        });

        $('[data-playlist-action="edit"]').on('click', e => {
            var data = { id: $('#txtPlaylistId').val(), name: $('#txtPlaylistName').val() },
                success = () => this.loadFunc(() => LoadingModal.hideLoading());

            $(this.modal).modal('hide').on('hidden.bs.modal', () => {
                LoadingModal.showLoading();
                $('#txtPlaylistId, #txtPlaylistName').val('');
                $.post('/Playlist/EditPlaylist', data, success);
            });
        });
    }
}