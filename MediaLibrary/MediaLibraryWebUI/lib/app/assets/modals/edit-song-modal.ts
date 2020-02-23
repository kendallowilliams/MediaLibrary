import HtmlControls from '../controls/html-controls';
import LoadingModal from '../modals/loading-modal';
import htmlControls from '../controls/html-controls';
import { ModalEventHandler, ModalEvent } from 'bootstrap';

export default class EditSongModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().EditSongModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', e => {
            const id = $(e.relatedTarget).attr('data-item-id'),
                success = data => {
                    this.clearEditSongModal();
                    $('#txtEditSongTitle').text(data.Title || 'Song')
                    $('#txtEditId').val(data.Id);
                    $('#txtEditTitle').val(data.Title);
                    $('#txtEditAlbum').val(data.Album);
                    $('#txtEditArtist').val(data.Artist);
                    $('#txtEditGenre').val(data.Genre);
                };

            $.get('Music/GetSong/' + id, success);
        });

        $('[data-song-action="save"]').on('click', e => {
            const data = 'Id=' + $('#txtEditId').val() + '&' +
                'Title=' + encodeURIComponent($('#txtEditTitle').val() as string) + '&' +
                'Album=' + encodeURIComponent($('#txtEditAlbum').val() as string) + '&' +
                'Artist=' + encodeURIComponent($('#txtEditArtist').val() as string) + '&' +
                'Genre=' + encodeURIComponent($('#txtEditGenre').val() as string),
                success = () => {
                    this.loadFunc(() => LoadingModal.hideLoading());
                };

            $(this.modal).modal('hide').on('hidden.bs.modal', () => {
                LoadingModal.showLoading();
                $.post('Music/UpdateSong', data, success);
            });
        });
    }

    private clearEditSongModal(): void {
        $('#txtEditSongTitle').text('Song')
        $('#txtEditId').val();
        $('#txtEditTitle').val();
        $('#txtEditAlbum').val();
        $('#txtEditArtist').val();
        $('#txtEditGenre').val();
    }
}