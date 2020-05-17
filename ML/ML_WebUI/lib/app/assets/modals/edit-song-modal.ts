import HtmlControls from '../controls/html-controls';
import LoadingModal from '../modals/loading-modal';
import MediaLibraryConfiguration from '../models/configurations/media-library-configuration';
import { MediaPages } from '../enums/enums';

export default class EditSongModal {
    private modal: HTMLElement;

    constructor(private mediaLibraryConfiguration: MediaLibraryConfiguration, private loadFunc: (mediaPage: MediaPages) => void = () => null) {
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
                    $('#txtEditPosition').val(data.Position);
                };

            $.get('Music/GetSong/' + id, success);
        });

        $('[data-song-action="save"]').on('click', e => {
            const data = 'Id=' + $('#txtEditId').val() + '&' +
                'Title=' + encodeURIComponent($('#txtEditTitle').val() as string) + '&' +
                'Album=' + encodeURIComponent($('#txtEditAlbum').val() as string) + '&' +
                'Artist=' + encodeURIComponent($('#txtEditArtist').val() as string) + '&' +
                'Genre=' + encodeURIComponent($('#txtEditGenre').val() as string) + '&' +
                'Position=' + encodeURIComponent($('#txtEditPosition').val() as string);

            $(this.modal).modal('hide').on('hidden.bs.modal', () => {
                LoadingModal.showLoading();
                $.post('Music/UpdateSong', data, () => this.loadFunc(this.mediaLibraryConfiguration.properties.SelectedMediaPage));
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