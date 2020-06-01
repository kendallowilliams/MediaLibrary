import BaseClass from "../../assets/models/base-class";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";
import HtmlControls from "../../assets/controls/html-controls";
import LoadingModal from "../../assets/modals/loading-modal";

export default class Search extends BaseClass {
    private searchTimeout: number;
    private searchDelay: number;

    constructor(private musicConfiguration: MusicConfiguration, private reload: () => void) {
        super();
        this.searchDelay = 1; 
    }

    initializeControls(): void {
        $('[data-back-button="search"]').on('click', () => this.goBack(this.reload));
        $('[data-music-action="search-music"]').on('click', this.search);
        $(HtmlControls.UIControls().SearchQuery).on('input', () => {
            if (this.searchTimeout) {
                clearTimeout(this.searchTimeout);
                this.searchTimeout = null;
            }

            this.searchTimeout = setTimeout(this.search, this.searchDelay * 1000);
        });
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
        const input = HtmlControls.UIControls().SearchQuery,
            query = $(input).val() as string,
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

        if (input.checkValidity()) {
            showHideLoading(true);

            $(containers.SearchAlbumsContainer).load('Music/SearchAlbums', { query: query }, function () {
                $(containers.SearchArtistsContainer).load('Music/SearchArtists', { query: query }, function () {
                    $(containers.SearchSongsContainer).load('Music/SearchSongs', { query: query }, function () {
                        showHideLoading(false);
                        $('[data-group-url]').each((index, element) => {
                            LoadingModal.showLoading();
                            $($(element).attr('data-target')).load($(element).attr('data-group-url'), () => {
                                $(element).trigger('click');
                                LoadingModal.hideLoading();
                            });
                        });
                    });
                });
            });

        } else {
            showHideLoading(false);
        }
    }
};