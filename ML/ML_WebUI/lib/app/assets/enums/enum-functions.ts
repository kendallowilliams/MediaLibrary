import { MediaPages, SeriesSort, AlbumSort, ArtistSort, MusicTabs, SongSort, MediaTypes, RepeatTypes, PlayerPages, PlaylistSort, PodcastSort, PodcastFilter, PlaylistTabs } from "./enums";

export function getMediaPagesEnum(page: string): MediaPages {
    let mediaPage: MediaPages;

    switch (page) {
        case 'Music':
            mediaPage = MediaPages.Music;
            break;
        case 'Playlist':
            mediaPage = MediaPages.Playlist;
            break;
        case 'Player':
            mediaPage = MediaPages.Player;
            break;
        case 'Podcast':
            mediaPage = MediaPages.Podcast;
            break;
        case 'Television':
            mediaPage = MediaPages.Television;
            break;
        case 'Home':
        default:
            mediaPage = MediaPages.Home;
            break;
    }

    return mediaPage;
};

export function getMediaPagesEnumString(page: MediaPages): string {
    let mediaPage: string;

    switch (page) {
        case MediaPages.Music:
            mediaPage = 'Music';
            break;
        case MediaPages.Playlist:
            mediaPage = 'Playlist';
            break;
        case MediaPages.Player:
            mediaPage = 'Player';
            break;
        case MediaPages.Podcast:
            mediaPage = 'Podcast';
            break;
        case MediaPages.Television:
            mediaPage = 'Television';
            break;
        case MediaPages.Home:
        default:
            mediaPage = 'Home';
            break;
    }

    return mediaPage;
};

export function getSeriesSortEnum(sort: string): SeriesSort {
    let seriesSort: SeriesSort;

    switch (sort) {
        case 'AtoZ':
        default:
            seriesSort = SeriesSort.AtoZ;
            break;
    }

    return seriesSort;
};

export function getAlbumSortEnum(sort: string): AlbumSort {
    let albumSort: AlbumSort;

    switch (sort) {
        case 'AtoZ':
        default:
            albumSort = AlbumSort.AtoZ;
            break;
    }

    return albumSort;
};

export function getArtistSortEnum(sort: string): ArtistSort {
    let artistSort: ArtistSort;

    switch (sort) {
        case 'AtoZ':
        default:
            artistSort = ArtistSort.AtoZ;
            break;
    }

    return artistSort;
};

export function getMusicTabEnum(tab: string): MusicTabs {
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
};

export function getMusicTabEnumString(tab: MusicTabs): string {
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
};

export function getSongSortEnum(sort: string): SongSort {
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
};

export function getPlayerPageEnum(page: string): PlayerPages {
    let playerPage: PlayerPages = PlayerPages.Index;

    switch (page) {
        case 'Audio':
            playerPage = PlayerPages.Audio;
            break;
        case 'Video':
            playerPage = PlayerPages.Video;
            break;
        case 'Index':
        default:
            playerPage = PlayerPages.Index;
            break;
    }

    return playerPage;
};

export function getRepeatTypesEnumString(page: RepeatTypes): string {
    let repeatType: string;

    switch (page) {
        case RepeatTypes.None:
            repeatType = 'None';
            break;
        case RepeatTypes.RepeatAll:
            repeatType = 'RepeatAll';
            break;
        case RepeatTypes.RepeatOne:
            repeatType = 'RepeatOne';
            break;
        default:
            repeatType = '';
            break;
    }

    return repeatType;
};

export function getMediaTypesEnum(type: string): MediaTypes {
    let mediaType: MediaTypes;

    switch (type) {
        case 'Television':
            mediaType = MediaTypes.Television;
            break;
        case 'Podcast':
            mediaType = MediaTypes.Podcast;
            break;
        case 'Song':
        default:
            mediaType = MediaTypes.Song;
            break;
    }

    return mediaType;
};

export function getMediaTypesEnumString(type: MediaTypes): string {
    let mediaType: string;

    switch (type) {
        case MediaTypes.Television:
            mediaType = 'Television';
            break;
        case MediaTypes.Podcast:
            mediaType = 'Podcast';
            break;
        case MediaTypes.Song:
        default:
            mediaType = 'Song';
            break;
    }

    return mediaType;
};

export function getPlaylistSortEnum(sort: string): PlaylistSort {
    let playlistSort: PlaylistSort;

    switch (sort) {
        case 'DateAdded':
            playlistSort = PlaylistSort.DateAdded;
            break;
        case 'AtoZ':
        default:
            playlistSort = PlaylistSort.AtoZ;
            break;
    }

    return playlistSort;
};

export function getPlaylistSortEnumString(sort: PlaylistSort): string {
    let playlistSort: string;

    switch (sort) {
        case PlaylistSort.DateAdded:
            playlistSort = 'DateAdded';
            break;
        case PlaylistSort.AtoZ:
        default:
            playlistSort = 'AtoZ';
            break;
    }

    return playlistSort;
};

export function getPodcastSortEnum(sort: string): PodcastSort {
    let podcastSort: PodcastSort;

    switch (sort) {
        case 'LastUpdateDate':
            podcastSort = PodcastSort.LastUpdateDate;
            break;
        case 'DateAdded':
            podcastSort = PodcastSort.DateAdded;
            break;
        case 'AtoZ':
        default:
            podcastSort = PodcastSort.AtoZ;
            break;
    }

    return podcastSort;
};

export function getPodcastFilterEnum(filter: string): PodcastFilter {
    let podcastFilter: PodcastFilter;

    switch (filter) {
        case 'Downloaded':
            podcastFilter = PodcastFilter.Downloaded;
            break;
        case 'Unplayed':
            podcastFilter = PodcastFilter.Unplayed;
            break;
        case 'All':
        default:
            podcastFilter = PodcastFilter.All;
            break;
    }

    return podcastFilter;
};

export function getPlaylistTabEnumString(tab: PlaylistTabs): string {
    let playlistTab: string;

    switch (tab) {
        case PlaylistTabs.Podcast:
            playlistTab = 'Podcast';
            break;
        case PlaylistTabs.Television:
            playlistTab = 'Television';
            break;
        case PlaylistTabs.Music:
        default:
            playlistTab = 'Music';
            break;
    }

    return playlistTab;
};

export function getPlaylistTabEnum(tab: string): PlaylistTabs {
    let playlistTab: PlaylistTabs;

    switch (tab) {
        case 'Podcast':
            playlistTab = PlaylistTabs.Podcast;
            break;
        case 'Television':
            playlistTab = PlaylistTabs.Television;
            break;
        case 'Music':
        default:
            playlistTab = PlaylistTabs.Music;
            break;
    }

    return playlistTab;
};