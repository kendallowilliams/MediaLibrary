import HtmlControls from "../controls/html-controls";
import LoadingModal from './loading-modal';

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

        $(this.modal).find('*[data-podcast-action="save"]').on('click', e => {
            LoadingModal.showLoading();
            $(this.modal).modal('hide').on('hidden.bs.modal', () => {
                $.post('/Podcast/AddPodcast', { rssFeed: $('#txtNewPodcast').val() }, () => {
                    this.loadFunc(() => LoadingModal.hideLoading());
                });
            });
        });
    }
}