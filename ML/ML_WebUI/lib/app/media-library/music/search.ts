import BaseClass from "../../assets/models/base-class";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";
import HtmlControls from "../../assets/controls/html-controls";

export default class Search extends BaseClass {
    constructor(private musicConfiguration: MusicConfiguration, private reload: () => void) {
        super();
    }

    initializeControls(): void {
        $('[data-back-button="search"]').on('click', () => this.goBack(this.reload));
        $('[data-action="search"]').on('click', this.search);
    }

    loadSearch(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Search;
        this.musicConfiguration.updateConfiguration(callback);
    }

    private goBack(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Index;
        this.musicConfiguration.updateConfiguration(callback);
    }

    private search() {
        const query = $(HtmlControls.UIControls().SearchQuery).val();

        alert(query);
    }
};