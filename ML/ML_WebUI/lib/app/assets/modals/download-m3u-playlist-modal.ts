import HtmlControls from '../controls/html-controls';

export default class DownloadM3UPlaylistModal {
    private modal: HTMLElement;

    constructor() {
        this.modal = HtmlControls.Modals().DownloadM3UPlaylistModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).on('show.bs.modal', e => {
            const $btn = $(e.relatedTarget),
                url = $btn.attr('data-href'),
                name = $btn.attr('data-playlist-name'),
                randomUrl = $btn.attr('data-random-href'),
                $yesBtn = $(this.modal).find('[data-answer="yes"]'),
                $noBtn = $(this.modal).find('[data-answer="no"]');

            $('#modalM3UPlaylistTitle').text(name);
            $yesBtn.attr('data-href', randomUrl);
            $noBtn.attr('data-href', url);
        });

        $(this.modal).on('hidden.bs.modal', e => {
            const $yesBtn = $(this.modal).find('[data-answer="yes"]'),
                $noBtn = $(this.modal).find('[data-answer="no"]');

            $('#modalM3UPlaylistTitle').text('');
            $yesBtn.attr('data-href', '');
            $noBtn.attr('data-href', '');
        });

        $(this.modal).find('[data-answer]').on('click', e => {
            window.location.href = $(e.currentTarget).attr('data-href');
            $(this.modal).modal('hide');
        });
    }
}