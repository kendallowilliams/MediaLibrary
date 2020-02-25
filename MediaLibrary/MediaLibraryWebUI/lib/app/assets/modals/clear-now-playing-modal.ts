import HtmlControls from '../controls/html-controls';
import MediaLibraryConfiguration from '../models/configurations/media-library-configuration';

export default class ClearNowPlayingModal {
    private modal: HTMLElement;

    constructor(private loadFunc: () => void = () => null) {
        this.modal = HtmlControls.Modals().ClearNowPlayingModal;
        this.initializeControls();
    }

    private initializeControls(): void {
        $(this.modal).find('[data-item-action="clear"]').on('click', e => {
            $(this.modal).modal('hide').on('hidden.bs.modal', () => $.get('Player/ClearNowPlaying', () => this.loadFunc()));
        });
    }
}