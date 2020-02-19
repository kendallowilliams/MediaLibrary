import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";
import HtmlControls from '../../assets/controls/html-controls';
import Artist from "./artist";
import Album from "./album";

export default class Music extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private artist: Artist;
    private album: Album;

    constructor(private musicConfiguration: MusicConfiguration) {
        super();
        this.mediaView = HtmlControls.Views.MediaView;
        this.artist = new Artist(musicConfiguration);
        this.album = new Album(musicConfiguration);
    }

    loadView(): void {
        $(this.mediaView).load('/Music/Index', () => null);
    }
}