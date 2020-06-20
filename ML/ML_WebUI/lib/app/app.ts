import MediaLibrary from './media-library/media-library';
import HtmlControls from './assets/controls/html-controls';

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
    }
}