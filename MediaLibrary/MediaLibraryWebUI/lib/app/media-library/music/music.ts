import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import MusicConfiguration from "../../assets/models/configurations/music-configuration";
import { MusicPages, AlbumSort, SongSort, ArtistSort, MusicTabs } from "../../assets/enums/enums";
import HtmlControls from '../../assets/controls/html-controls';
import Artist from "./artist";
import Album from "./album";
import LoadingModal from "../../assets/modals/loading-modal";
import IMusicConfiguration from "../../assets/interfaces/music-configuration-interface";

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
        const properties: IMusicConfiguration = this.musicConfiguration.properties,
            playSingle: boolean = properties.SelectedMusicTab === MusicTabs.Songs && properties.SelectedMusicPage === MusicPages.Index,
            success: () => void = () => {
            $('[data-play-id]').on('click', e => this.playFunc(e.target as HTMLButtonElement, playSingle));
            callback();
        }

        $(this.mediaView).load('/Music/Index', success);
    }

    refresh(): void {
        $.post('/Music/Refresh', () => this.loadView.call(this));
    }

    sortChanged(sortType: string, select: HTMLSelectElement): void {
        if (sortType === 'SelectedAlbumSort') {
            this.musicConfiguration.properties.SelectedAlbumSort = this.getAlbumSortEnum($(select).val() as string);
        } else if (sortType === 'SelectedArtistSort') {
            this.musicConfiguration.properties.SelectedArtistSort = this.getArtistSortEnum($(select).val() as string);
        } else if (sortType === 'SelectedSongSort') {
            this.musicConfiguration.properties.SelectedSongSort = this.getSongSortEnum($(select).val() as string);
        }

        this.musicConfiguration.updateConfiguration(() => this.loadView.call(this));
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