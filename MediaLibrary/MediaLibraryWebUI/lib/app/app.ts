import MediaLibrary from './media-library/media-library';

export class App {
    private mediaLibrary: MediaLibrary;

    constructor() {
        this.mediaLibrary = new MediaLibrary();
    }
}

let app = new App();