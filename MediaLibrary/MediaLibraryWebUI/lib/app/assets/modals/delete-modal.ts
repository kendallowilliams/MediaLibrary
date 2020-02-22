import HtmlControls from '../../assets/controls/html-controls';
import LoadingModal from '../modals/loading-modal';

export default class DeleteModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().DeleteModal;
        this.initializeControls();
    }

    initializeControls(): void {
        $(this.modal).on('show.bs.modal', function (e) {
            var $btn = $(e.relatedTarget),
                url = $btn.attr('data-delete-action'),
                refreshView = $btn.attr('data-refresh-view'),
                type = $btn.attr('data-delete-type'),
                $button = $('#btnDeleteItem');

            $('#modalDeleteItemTitle').text('Delete ' + type);
            $button.attr('data-delete-url', url);
            $button.attr('data-refresh-view', refreshView);
        });

        $(this.modal).on('hide.bs.modal', function () {
            var $button = $('#btnDeleteItem');

            $('#modalDeleteItemTitle').text('Delete');
            $button.attr('data-delete-url', '');
            $button.attr('data-refresh-view', '');
        });

        $(this.modal).find('[data-item-action="delete"]').on('click', e => {
            const $btn = $(e.target),
                url = $btn.attr('data-delete-url'),
                view = $btn.attr('data-refresh-view'),
                success = () => {
                    this.loadFunc(() => LoadingModal.hideLoading());
                };

            LoadingModal.showLoading();
            $(this.modal).modal('hide').on('hidden.bs.modal', function () {
                $.get(url, success);
            });
        });
    }
}