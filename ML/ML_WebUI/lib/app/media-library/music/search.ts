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
        private playFunc: (btn: HTMLButtonElement, single: boolean) => void,
        private loadAlbum: (id: number, callback: () => void) => void,
        private loadArtist: (id: number, callback: () => void) => void,
        private updateActiveMediaFunc: () => void = () => null) {
        super();
        this.searchDelay = 1; 
    }

    initializeControls(): void {
        $('[data-back-button="search"]').on('click', () => {
            this.musicConfiguration.properties.PreviousSearchQuery = '';
            this.musicConfiguration.updateConfiguration(() => this.goBack(this.reload));
        });
        $('[data-music-action="search-music"]').on('click', this.search.bind(this));
        $(HtmlControls.UIControls().SearchQuery).on('input', () => {
            if (this.searchTimeout) {
                clearTimeout(this.searchTimeout);
                this.searchTimeout = null;
            }

            this.searchTimeout = setTimeout(this.search.bind(this), this.searchDelay * 1000);
        });
        $(HtmlControls.UIControls().SearchQuery).val(this.musicConfiguration.properties.PreviousSearchQuery);
    }

    loadSearch(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Search;
        this.musicConfiguration.updateConfiguration(callback);
    }

    private goBack(callback: () => void = () => null): void {
        this.musicConfiguration.properties.SelectedMusicPage = MusicPages.Index;
        this.musicConfiguration.updateConfiguration(callback);
    }

    search() {
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

        if (query && query.length >= input.minLength) {
            showHideLoading(true);
            LoadingModal.showLoading();

            this.musicConfiguration.properties.PreviousSearchQuery = query;
            this.musicConfiguration.updateConfiguration(() => {
                $(containers.SearchAlbumsContainer).load('Music/SearchAlbums', { query: query }, () => {
                    $('[data-album-id]').on('click', _e => this.loadAlbum(parseInt($(_e.currentTarget).attr('data-album-id')), this.reload));

                    $(containers.SearchArtistsContainer).load('Music/SearchArtists', { query: query }, () => {
                        $('[data-artist-id]').on('click', _e => this.loadArtist(parseInt($(_e.currentTarget).attr('data-artist-id')), this.reload));

                        $(containers.SearchSongsContainer).load('Music/SearchSongs', { query: query }, () => {
                            $(containers.SearchSongsContainer).find('[data-play-id]').on('click', e => {
                                this.playFunc(e.currentTarget as HTMLButtonElement, true);
                            });

                            this.updateActiveMediaFunc();
                            showHideLoading(false);
                            LoadingModal.hideLoading();
                        });
                    });
                });
            });
        } else {
            LoadingModal.showLoading();
            this.musicConfiguration.properties.PreviousSearchQuery = '';
            this.musicConfiguration.updateConfiguration(() => {
                $(containers.SearchAlbumsContainer).html('<div>No albums.</div>');
                $(containers.SearchArtistsContainer).html('<div>No artists.</div>');
                $(containers.SearchSongsContainer).html('<div>No songs.</div>');
                showHideLoading(false);
                LoadingModal.hideLoading();
            });
        }
    }
};