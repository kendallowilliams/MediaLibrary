import HtmlControls from '../controls/html-controls';
import LoadingModal from '../modals/loading-modal';

export default class EditSongModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().EditSongModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', e => {
            $('#inpNewSong').val('');
        });

        $('[data-song-action="save"]').on('click', e => {
            var data = new FormData(),
                success = () => this.loadFunc(() => LoadingModal.hideLoading());

            $(this.modal).modal('hide');

            if ($('#inpNewSong').prop('files').length > 0) {
                LoadingModal.showLoading();
                data.append("file", $('#inpNewSong').prop('files')[0]);
                $.ajax({
                    url: '/Music/Upload',
                    data: data,
                    processData: false,
                    contentType: false,
                    type: 'POST',
                    success: success
                });
            }
        });
    }
}