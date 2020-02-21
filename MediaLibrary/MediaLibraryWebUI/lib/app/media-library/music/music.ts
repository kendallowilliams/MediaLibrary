import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages, AlbumSort, SongSort, ArtistSort, MusicTabs } from "../../assets/enums/enums";
import HtmlControls from '../../assets/controls/html-controls';
import Artist from "./artist";
import Album from "./album";
import LoadingModal from "../../assets/modals/loading-modal";
import IMusicConfiguration from "../../assets/interfaces/music-configuration-interface";
import { loadTooltips, disposeTooltips } from '../../assets/utilities/bootstrap';

export default class Music extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;
    private artist: Artist;
    private album: Album;

    constructor(private musicConfiguration: MusicConfiguration, private playFunc: (btn: HTMLButtonElement, single: boolean) => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
        this.artist = new Artist(musicConfiguration);
        this.album = new Album(musicConfiguration);
    }

    loadView(callback: () => void = () => null): void {
        const success: () => void = () => {
            this.initializeControls();
            $('[data-music-tab="' + this.getMusicTabEnumString(this.musicConfiguration.properties.SelectedMusicTab) + '"]').tab('show');
            callback();
        }; 

        $(this.mediaView).load('/Music/Index', success);
    }

    private initializeControls(): void {
        const properties: IMusicConfiguration = this.musicConfiguration.properties,
            playSingle: boolean = properties.SelectedMusicTab === MusicTabs.Songs && properties.SelectedMusicPage === MusicPages.Index;

        $('[data-play-id]').on('click', e => this.playFunc(e.target as HTMLButtonElement, playSingle));
        $('[data-back-button="artist"]').on('click', () => this.artist.goBack(() => this.loadView.call(this)));
        $('[data-back-button="album"]').on('click', () => this.album.goBack(() => this.loadView.call(this)));
        $('[data-album-id]').on('click', _e => this.album.loadAlbum(parseInt($(_e.currentTarget).attr('data-album-id')), () => this.loadView()));
        $('[data-artist-id]').on('click', _e => this.artist.loadArtist(parseInt($(_e.currentTarget).attr('data-artist-id')), () => this.loadView()));

        $(HtmlControls.UIControls().MusicTabList).find('*[data-toggle="tab"]').on('shown.bs.tab', e => {
            const $newTab = $(e.target),
                $oldTab = $(e.relatedTarget),
                $newView = $($newTab.attr('href')),
                $oldView = $($oldTab.attr('href')),
                url = $newView.attr('data-load-url'),
                success = () => {
                    LoadingModal.hideLoading();
                    loadTooltips($newView[0]);

                    $('[data-group-url]').on('click', function () {
                        const $btn = $(this),
                            url = $btn.attr('data-group-url');
                        if (url) {
                            LoadingModal.showLoading();
                            $($btn.attr('data-target')).load(url, function () {
                                loadTooltips($($btn.attr('data-target'))[0]);
                                LoadingModal.hideLoading();
                                $btn.attr('data-group-url', '');
                            });
                        }
                    });
                    $('[data-album-id]').on('click', _e => this.album.loadAlbum(parseInt($(_e.currentTarget).attr('data-album-id')), () => this.loadView()));
                    $('[data-artist-id]').on('click', _e => this.artist.loadArtist(parseInt($(_e.currentTarget).attr('data-artist-id')), () => this.loadView()));
                    $('[data-group-url][data-target="#collapse-songs-0"]').trigger('click');
                };
            $(HtmlControls.UIControls().MusicTabList).find('*[data-sort-tab]').each((index, _btn) => {
                if ($(_btn).attr('data-sort-tab') === $newTab.attr('id')) {
                    $(_btn).removeClass('d-none');
                } else {
                    $(_btn).addClass('d-none');
                }
            });
            LoadingModal.showLoading();
            this.musicConfiguration.properties.SelectedMusicTab = this.getMusicTabEnum($newTab.attr('data-music-tab'));
            disposeTooltips($newView[0]);
            this.musicConfiguration.updateConfiguration(() => $newView.load(url, success));
        });

        $(this.mediaView).find('*[data-sort-type]').on('change', e => {
            const select = e.target,
                sortType: string = $(e.target).attr('data-sort-type');

            if (sortType === 'SelectedAlbumSort') {
                this.musicConfiguration.properties.SelectedAlbumSort = this.getAlbumSortEnum($(select).val() as string);
            } else if (sortType === 'SelectedArtistSort') {
                this.musicConfiguration.properties.SelectedArtistSort = this.getArtistSortEnum($(select).val() as string);
            } else if (sortType === 'SelectedSongSort') {
                this.musicConfiguration.properties.SelectedSongSort = this.getSongSortEnum($(select).val() as string);
            }

            this.musicConfiguration.updateConfiguration(() => this.loadView());
        });
    }

    refresh(): void {
        LoadingModal.showLoading();
        $.post('/Music/Refresh', () => this.loadView(() => LoadingModal.hideLoading()));
    }

    private getAlbumSortEnum(sort: string): AlbumSort {
        let albumSort: AlbumSort;

        switch (sort) {
            case 'AtoZ':
            default:
                albumSort = AlbumSort.AtoZ;
                break;
        }

        return albumSort;
    }

    private getArtistSortEnum(sort: string): ArtistSort {
        let artistSort: ArtistSort;

        switch (sort) {
            case 'AtoZ':
            default:
                artistSort = ArtistSort.AtoZ;
                break;
        }

        return artistSort;
    }

    private getMusicTabEnum(tab: string): MusicTabs {
        let musicTab: MusicTabs;

        switch (tab) {
            case 'Artists':
                musicTab = MusicTabs.Artists;
                break;
            case 'Albums':
                musicTab = MusicTabs.Albums;
                break;
            case 'Songs':
            default:
                musicTab = MusicTabs.Songs;
                break;
        }

        return musicTab;
    }

    private getMusicTabEnumString(tab: MusicTabs): string {
        let musicTab: string;

        switch (tab) {
            case MusicTabs.Artists:
                musicTab = 'Artists';
                break;
            case MusicTabs.Albums:
                musicTab = 'Albums';
                break;
            case MusicTabs.Songs:
            default:
                musicTab = 'Songs';
                break;
        }

        return musicTab;
    }

    private getSongSortEnum(sort: string): SongSort {
        let songSort: SongSort;

        switch (sort) {
            case 'Album':
                songSort = SongSort.Album;
                break;
            case 'Artist':
                songSort = SongSort.Artist;
                break;
            case 'DateAdded':
                songSort = SongSort.DateAdded;
                break;
            case 'Genre':
                songSort = SongSort.Genre;
                break;
            case 'AtoZ':
            default:
                songSort = SongSort.AtoZ;
                break;
        }

        return songSort;
    }
}