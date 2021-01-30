import MediaLibrary from './media-library/media-library';
import HtmlControls from './assets/controls/html-controls';
import * as MessageBox from './assets/utilities/message-box';

export default class App {
    private mediaLibrary: MediaLibrary;

    constructor() {
        this.initialize();
        this.mediaLibrary = new MediaLibrary();
    }

    private initialize(): void {
        document.onfullscreenchange = () => {
            const players = HtmlControls.Players();

            $([players.MusicPlayer, players.VideoPlayer]).prop('controls', document.fullscreen);
        };
        window.onerror = (event: Event | string, source?: string, lineno?: number, colno?: number, error?: Error) => {
            MessageBox.showError('Error', error.message);
        };
    }
}