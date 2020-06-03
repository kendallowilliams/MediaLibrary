import BaseClass from "../../assets/models/base-class";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages } from "../../assets/enums/enums";
import HtmlControls from "../../assets/controls/html-controls";
import LoadingModal from "../../assets/modals/loading-modal";

export default class Search extends BaseClass {
    private searchTimeout: number;
    private searchDelay: number;

    constructor(private musicConfiguration: MusicConfiguration,
        private reload: () => void,
        private playFunc: (btn: HTMLButtonElement, single: boolean) => void) {
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

            this.searchTimeout = setTimeout(this.search.bind(this), this.searchDelay * 1000);
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

    private search() {
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
                $btn.prop('disabled', searching);
            },
            containers = HtmlControls.Containers();

        if (input.checkValidity()) {
            showHideLoading(true);

            $(containers.SearchAlbumsContainer).load('Music/SearchAlbums', { query: query }, () => {
                $(containers.SearchArtistsContainer).load('Music/SearchArtists', { query: query }, () => {
                    $(containers.SearchSongsContainer).load('Music/SearchSongs', { query: query }, () => {
                        $(containers.SearchSongsContainer).find('[data-play-id]').on('click', e => {
                            this.playFunc(e.currentTarget as HTMLButtonElement, true);
                        });

                        showHideLoading(false);
                    });
                });
            });

        } else {
            $(containers.SearchAlbumsContainer).html('<div>No albums.</div>');
            $(containers.SearchArtistsContainer).html('<div>No artists.</div>');
            $(containers.SearchSongsContainer).html('<div>No songs.</div>');
            showHideLoading(false);
        }
    }
};