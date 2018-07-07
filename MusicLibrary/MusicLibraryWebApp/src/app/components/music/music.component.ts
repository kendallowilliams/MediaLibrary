import { Component, OnInit, Input } from '@angular/core';

import { AlbumService } from '../../services/album.service';
import { ArtistService } from '../../services/artist.service';
import { TrackService } from '../../services/track.service';
import { GenreService } from '../../services/genre.service';

import { Album } from '../../shared/models/album.model';
import { Track } from '../../shared/models/track.model';
import { Artist } from '../../shared/models/artist.model';
import { Genre } from '../../shared/models/genre.model';
import { ActivatedRoute } from '@angular/router';

import { TrackSortEnum, AlbumSortEnum, MusicTabEnum } from './enums/music-enum';
import { ITrackList, IAlbumList, IArtistList } from '../../shared/interfaces/music.interface';
import { TrackListComponent } from './track-list/track-list.component';
import { TrackComponent } from './track/track.component';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  public MusicTabs = MusicTabEnum;

  @Input() musicCount: number;

  letters = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  tracks: Track[];
  artists: Artist[];
  albums: Album[];
  genres: Genre[];
  currentTrackSort: TrackSortEnum;
  currentAlbumSort: AlbumSortEnum;
  trackSortOptions: any[];
  albumSortOptions: any[];
  trackSortGroups: ITrackList[];
  artistSortGroups: IArtistList[];
  albumSortGroups: IAlbumList[];
  selectMusicTab: MusicTabEnum;

  constructor(private trackService: TrackService, private artistService: ArtistService,
    private albumService: AlbumService, private genreService: GenreService,
    private route: ActivatedRoute) {
    this.musicCount = 0;
  }

  ngOnInit() {
    this.getTracks();
    this.getArtists();
    this.getAlbums();
    this.getGenres();
    this.updateTracks();
    this.updateAlbums();
    this.updateMusicTab(MusicTabEnum.Songs);
  }

  updateMusicTab(musicTab: MusicTabEnum): void {
    this.selectMusicTab = musicTab;

    switch (musicTab) {
      case MusicTabEnum.Songs:
        this.musicCount = this.tracks.length;
        break;
      case MusicTabEnum.Artists:
        this.artistSortGroups = this.getArtistSortGroups(); // currently atoz only option
        this.musicCount = this.artists.length;
        break;
      case MusicTabEnum.Albums:
        this.musicCount = this.albums.length;
        break;
      case MusicTabEnum.None:
      default:
        this.albumSortGroups = [];
        break;
    }
  }

  updateTrackSort(trackSort: TrackSortEnum): void {
    if (this.currentTrackSort !== trackSort) {
      this.currentTrackSort = trackSort;
      this.trackSortGroups = this.getTrackSortGroups();
    }
  }

  updateAlbumSort(albumSort: AlbumSortEnum): void {
    if (this.currentAlbumSort !== albumSort) {
      this.currentAlbumSort = albumSort;
      this.albumSortGroups = this.getAlbumSortGroups();
    }
  }

  getTrackSortGroups(): ITrackList[] {
    let groups: ITrackList[] = [];

    switch (this.currentTrackSort) {
      case TrackSortEnum.Album:
        const albums = this.albums.map(album => album.title);
        groups = albums.filter((album, index, _albums) => _albums.findIndex(item => item === album) === index)
          .map(album => ({
            title: album,
            tracks: this.tracks.filter(track => track.album === album)
          }));
        break;
      case TrackSortEnum.Artist:
        const artists = this.artists.map(artist => artist.name);
        groups = artists.filter((artist, index, _artists) => _artists.findIndex(item => item === artist) === index)
          .map(artist => ({
            title: artist,
            tracks: this.tracks.filter(track => track.artist === artist)
          }));
        break;
      case TrackSortEnum.AtoZ:
        groups = ['&', '#'].concat(this.letters)
          .map(char => ({
            title: char,
            tracks: this.getTracksAtoZ(char)
          }));
        break;
      case TrackSortEnum.DateAdded:
        const dates = this.tracks.map(track => track.createDate.toDateString()).sort().reverse();
        groups = dates.filter((date, index, _dates) => _dates.findIndex(item => item === date) === index)
          .map(date => ({
            title: date,
            tracks: this.tracks.filter(track => track.createDate.toDateString() === date)
          }));
        break;
      case TrackSortEnum.None:
      default:
        groups = [];
        break;
    }

    groups.forEach(group => group.height = (group.tracks.length * TrackComponent.TrackHeight) + TrackListComponent.HeaderHeight);

    return groups.filter(group => group.tracks.length > 0);
  }

  getArtistSortGroups(): IArtistList[] {
    return ['&', '#'].concat(this.letters).map(char => ({
      title: char,
      artists: this.getArtistsAtoZ(char)
    }));
  }

  getAlbumSortGroups(): IAlbumList[] {
    let groups: IAlbumList[] = [];

    switch (this.currentAlbumSort) {
      case AlbumSortEnum.Artist:
        const artists = this.artists.map(artist => artist.name);
        groups = artists.filter((artist, index, _artists) => _artists.findIndex(item => item === artist) === index)
          .map(artist => ({
            title: artist,
            albums: this.albums.filter(album => album.artist === artist)
          }));
        break;
      case AlbumSortEnum.AtoZ:
        groups = ['&', '#'].concat(this.letters)
          .map(char => ({
            title: char,
            albums: this.getAlbumsAtoZ(char)
          }));
        break;
      case AlbumSortEnum.DateAdded:
        const dates = this.albums.map(album => album.createDate.toDateString()).sort().reverse();
        groups = dates.filter((date, index, _dates) => _dates.findIndex(item => item === date) === index)
          .map(date => ({
            title: date.toString(),
            albums: this.albums.filter(album => album.createDate.toDateString() === date)
          }));
        break;
      case AlbumSortEnum.ReleaseYear:
        const years = this.albums.map(album => album.year).sort().reverse();
        groups = years.filter((year, index, _years) => _years.findIndex(item => item === year) === index)
          .map(year => ({
            title: year === 0 ? 'unknown' : year.toString(),
            albums: this.albums.filter(album => album.year === year)
          }));
        break;
      case AlbumSortEnum.None:
      default:
        groups = [];
        break;
    }

    return groups.filter(group => group.albums.length > 0);
  }

  getTracksAtoZ(char: string): Track[] {
    let tracks = [];

    switch (char) {
      case '&':
        tracks = this.tracks.filter(track => isNaN(parseInt(track.title[0], 10)) &&
          !this.letters.includes(track.title[0].toUpperCase()));
        break;
      case '#':
        tracks = this.tracks.filter(track => !isNaN(parseInt(track.title[0], 10)));
        break;
      default:
        tracks = this.tracks.filter(track => track.title[0].toUpperCase() === char);
        break;
    }

    return tracks;
  }

  getArtistsAtoZ(char: string): Artist[] {
    let artists = [];

    switch (char) {
      case '&':
        artists = this.artists.filter(artist => isNaN(parseInt(artist.name[0], 10)) &&
          !this.letters.includes(artist.name[0].toUpperCase()));
        break;
      case '#':
        artists = this.artists.filter(artist => !isNaN(parseInt(artist.name[0], 10)));
        break;
      default:
        artists = this.artists.filter(artist => artist.name[0].toUpperCase() === char);
        break;
    }

    return artists;
  }

  getAlbumsAtoZ(char: string): Album[] {
    let albums = [];

    switch (char) {
      case '&':
        albums = this.albums.filter(album => isNaN(parseInt(album.title[0], 10)) &&
          !this.letters.includes(album.title[0].toUpperCase()));
        break;
      case '#':
        albums = this.albums.filter(album => !isNaN(parseInt(album.title[0], 10)));
        break;
      default:
        albums = this.albums.filter(album => album.title[0].toUpperCase() === char);
        break;
    }

    return albums;
  }

  updateTracks(): void {
    this.tracks.forEach(track => {
      track.album = this.getAlbumTitleById(track.albumId);
      track.artist = this.getArtistNameById(track.artistId);
      track.genre = this.getGenreNameById(track.genreId);
    });
  }

  updateAlbums(): void {
    this.albums.forEach(album => {
      album.artist = this.getArtistNameById(album.artistId);
      album.genre = this.getGenreNameById(album.genreId);
    });
  }

  getGenres(): void {
    this.genres = this.route.snapshot.data['genres'];
  }

  getTracks(): void {
    this.tracks = this.route.snapshot.data['tracks'];
  }

  getAlbums(): void {
    this.albums = this.route.snapshot.data['albums'];
  }

  getArtists(): void {
    this.artists = this.route.snapshot.data['artists'];
  }

  getGenreNameById(id: number): string {
    const foundGenre = this.genres.find(genre => genre.id === id);
    return foundGenre !== undefined && foundGenre !== null ? foundGenre.name : '';
  }

  getAlbumTitleById(id: number): string {
    const foundAlbum = this.albums.find(album => album.id === id);
    return foundAlbum !== undefined && foundAlbum !== null ? foundAlbum.title : '';
  }

  getArtistNameById(id: number): string {
    const foundArtist = this.artists.find(artist => artist.id === id);
    return foundArtist !== undefined && foundArtist !== null ? foundArtist.name : '';
  }

  handleScroll(thisArg: MusicComponent, height: number, scrollTop: number): void {
    const parentTop = scrollTop,
          parentBottom = parentTop + height;

    thisArg.trackSortGroups.reduce((acc, current) => {
      const listTop = acc,
            listBottom = acc + current.height;

      if ((listTop >= parentTop && listTop <= parentBottom) ||
          (listBottom >= parentTop && listBottom <= parentBottom) ||
          (parentTop >= listTop && parentTop <= listBottom) ||
          (parentBottom >= listTop && parentBottom <= listBottom)) {
        current.showTracks();
      } else {
        current.hideTracks();
      }

      return listBottom;
    }, 0);
  }
}
