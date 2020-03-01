import MediaLibrary from './media-library/media-library';

export default class App {
    private mediaLibrary: MediaLibrary;

    constructor() {
        this.mediaLibrary = new MediaLibrary();
    }
}