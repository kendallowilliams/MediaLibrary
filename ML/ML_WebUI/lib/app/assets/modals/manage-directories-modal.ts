import HtmlControls from '../controls/html-controls';
import MediaLibraryConfiguration from '../models/configurations/media-library-configuration';
import LoadingModal from './loading-modal';

export default class ManageDirectoriesModal {
    private modal: HTMLElement;

    constructor(private loadFunc: (callback: () => void) => void = () => null) {
        this.modal = HtmlControls.Modals().ManageDirectoriesModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', e => {
            this.loadMusicDirectory('');
        });

        $(this.modal).on('hide.bs.modal', e => {
            $(this.modal).find('.modal-body').html('');
        });
    }

    private loadMusicDirectory(path: string): void {
        LoadingModal.showLoading();
        $(this.modal).find('.modal-body').load('Music/GetMusicDirectory?path=' + path, () => {
            $(this.modal).find('[data-directory-action="get"]').on('click', e => {
                const path = $(e.currentTarget).attr('data-directory-path');

                this.loadMusicDirectory(encodeURIComponent(path));
            });
            $(this.modal).find('[data-directory-action-type="remove"]').on('click', e => {
                this.removeMusicDirectory(e.currentTarget);
            });
            $(this.modal).find('[data-directory-action-type="add"]').on('click', e => {
                this.addMusicDirectory(e.currentTarget);
            });
            LoadingModal.hideLoading();
        });
    }

    private addMusicDirectory(btn: HTMLElement): void {
        const $btn = $(btn),
            action = $btn.attr('data-directory-action'),
            path = $btn.attr('data-directory-path');

        LoadingModal.showLoading();
        $(this.modal).modal('hide');
        $.post(action, { path: path }, () => {
            LoadingModal.hideLoading();
        });
    }

    private removeMusicDirectory(btn: HTMLElement): void {
        const $btn = $(btn),
            action = $btn.attr('data-directory-action'),
            id = $btn.attr('data-path-id');

        LoadingModal.showLoading();
        $(this.modal).modal('hide');
        $.post(action, { id: id }, () => this.loadFunc(() => LoadingModal.hideLoading()));
    }
}