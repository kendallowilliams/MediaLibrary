import HtmlControls from "../controls/html-controls";
import LoadingModal from "./loading-modal";

export default class AddToPlaylistModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().AddToPlaylistModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', function (e) {
            var $btn = $(e.relatedTarget),
                url = $btn.attr('data-playlist-url'),
                id = $btn.attr('data-item-id');
            $('[data-playlist-item="enabled"]').attr('data-playlist-url', url);
            $('[data-playlist-item="enabled"]').attr('data-item-id', id);
        });

        $(this.modal).on('hide.bs.modal', function (e) {
            $('[data-playlist-item="enabled"]').attr('data-playlist-url', '');
            $('[data-playlist-item="enabled"]').attr('data-item-id', '');
        });

        $('[data-playlist-action="add"]').on('click', e => {
            var $btn = $(e.target),
                url = $btn.attr('data-playlist-url'),
                id = $btn.attr('data-item-id'),
                playlistId = $btn.attr('data-playlist-id');

            LoadingModal.showLoading()
            $(this.modal).modal('hide')
            $.post(url, { playlistId, itemId: id }, () => LoadingModal.hideLoading());
        });
    }
}