import BaseClass from "../../assets/models/base-class";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";
import HtmlControls from "../../assets/controls/html-controls";

export default class Search extends BaseClass {
    private searchTimeout: number;

    constructor(private musicConfiguration: MusicConfiguration, private reload: () => void) {
        super();
    }

    initializeControls(): void {
        $('[data-back-button="search"]').on('click', () => this.goBack(this.reload));
        $('[data-music-action="search-music"]').on('click', this.search);
    }

    loadSearch(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Search;
        this.musicConfiguration.updateConfiguration(callback);
    }

    private goBack(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Index;
        this.musicConfiguration.updateConfiguration(callback);
    }

    private async search() {
        const query = $(HtmlControls.UIControls().SearchQuery).val() as string,
            $btn = $('[data-music-action="search-music"]'),
            showHideLoading = searching => {
                if (searching) {
                    $btn.find('[data-searching-visible="false"]').addClass('d-none');
                    $btn.find('[data-searching-visible="true"]').removeClass('d-none');
                } else {
                    $btn.find('[data-searching-visible="true"]').addClass('d-none');
                    $btn.find('[data-searching-visible="false"]').removeClass('d-none');
                }
            },
            containers = HtmlControls.Containers();

        if (query.length > 3) {
            showHideLoading(true);

            $(containers.SearchAlbumsContainer).load('Music/SearchAlbums', { query: query }, function () {
                $(containers.SearchArtistsContainer).load('Music/SearchArtists', { query: query }, function () {
                    $(containers.SearchSongsContainer).load('Music/SearchSongs', { query: query }, function () {
                        showHideLoading(false);
                    });
                });
            });

        } else {
            showHideLoading(false);
        }
    }
};