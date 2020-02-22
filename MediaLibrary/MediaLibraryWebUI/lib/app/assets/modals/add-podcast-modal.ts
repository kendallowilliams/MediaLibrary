import HtmlControls from "../controls/html-controls";
import LoadingModal from './loading-modal'

export default class AddNewPodcastModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().NewPodcastModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', e => {
            $('#txtNewPodcast').val('');
        });

        $('[data-podcast-action="save"]').on('click', e => {
            var success = () => {
                LoadingModal.hideLoading();
                this.loadFunc(() => LoadingModal.hideLoading());
            }

            $(this.modal).modal('hide').on('hide.bs.modal', () => {
                LoadingModal.showLoading();
                $.post('/Podcast/AddPodcast', { rssFeed: $('#txtNewPodcast').val() }, success);
            });
        });
    }
}