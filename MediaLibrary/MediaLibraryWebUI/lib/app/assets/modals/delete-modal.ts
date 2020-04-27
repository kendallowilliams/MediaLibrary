import HtmlControls from '../../assets/controls/html-controls';
import LoadingModal from '../modals/loading-modal';
import MediaLibraryConfiguration from '../models/configurations/media-library-configuration';
import { MediaPages } from '../enums/enums';

export default class DeleteModal {
    private modal: HTMLElement;

    constructor(private mediaLibraryConfiguration: MediaLibraryConfiguration, private loadFunc: (mediaPage: MediaPages) => void = () => null) {
        this.modal = HtmlControls.Modals().DeleteModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', e => {
            const $btn = $(e.relatedTarget),
                url = $btn.attr('data-delete-action'),
                type = $btn.attr('data-delete-type'),
                $button = $(this.modal).find('[data-item-action="delete"]');

            $('#modalDeleteItemTitle').text('Delete ' + type);
            $button.attr('data-delete-url', url);
        });

        $(this.modal).on('hide.bs.modal', e => {
            const $button = $(this.modal).find('[data-item-action="delete"]');

            $('#modalDeleteItemTitle').text('Delete');
            $button.attr('data-delete-url', '');
        });

        $(this.modal).find('[data-item-action="delete"]').on('click', e => {
            const $btn = $(e.currentTarget),
                url = $btn.attr('data-delete-url');
            
            LoadingModal.showLoading();
            $.get(url, () => {
                this.loadFunc(this.mediaLibraryConfiguration.properties.SelectedMediaPage);
                LoadingModal.hideLoading();
            });
        });
    }
}