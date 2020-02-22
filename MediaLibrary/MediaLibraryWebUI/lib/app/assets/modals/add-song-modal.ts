import HtmlControls from "../controls/html-controls";
import LoadingModal from "./loading-modal";

export default class AddNewSongModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().NewSongModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', function (e) {
            $('#inpNewSong').val('');
        });

        $('[data-song-action="upload"]').on('click', e => {
            var data = new FormData(),
                success = () => this.loadFunc(() => LoadingModal.hideLoading());

            $(this.modal).modal('hide');
            if ($('#inpNewSong').prop('files').length > 0) {
                LoadingModal.showLoading();
                data.append("file", $('#inpNewSong').prop('files')[0]);
                $.ajax({
                    url: '@Url.Action("Upload", "Music")',
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