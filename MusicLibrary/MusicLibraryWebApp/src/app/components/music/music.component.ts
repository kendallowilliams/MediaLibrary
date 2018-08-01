import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList } from '@angular/core';

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
import { ITrackList, IAlbumList, IArtistList, IScrollData } from '../../shared/interfaces/music.interface';
import { AppService } from '../../services/app.service';
import { Observable } from '../../../../node_modules/rxjs';
import { TrackListComponent } from './track-list/track-list.component';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  public MusicTabs = MusicTabEnum;

  @Input() musicCount: number;
  @Output() play: EventEmitter<number> = new EventEmitter<number>();
  @ViewChildren(TrackListComponent) children = new QueryList<TrackListComponent>();

  letters = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  tracks: Track[] = [];
  artists: Artist[] = [];
  albums: Album[] = [];
  genres: Genre[] = [];
  currentTrackSort: TrackSortEnum;
  currentAlbumSort: AlbumSortEnum;
  trackSortOptions: any[] = [];
  albumSortOptions: any[] = [];
  trackSortLists$: Observable<ITrackList[]>;
  artistSortGroups: IArtistList[] = [];
  albumSortGroups: IAlbumList[] = [];
  selectMusicTab: MusicTabEnum;
  scrollData: IScrollData = { top: 0, height: 0, timeout: 200 };

  constructor(private trackService: TrackService, private artistService: ArtistService,
    private albumService: AlbumService, private genreService: GenreService,
    private route: ActivatedRoute, private appService: AppService) {
    this.musicCount = 0;
    this.appService.musicComponent = this;
  }

  ngOnInit() {
    this.getArtists();
    this.getGenres();

    this.artistSortGroups = this.getArtistSortGroups();

    this.albumService.getAlbums().subscribe(albums => {
      this.albums = albums;
      this.albumSortGroups = this.getAlbumSortGroups();
      this.updateAlbums();
    });

    this.trackService.getTracks().subscribe(tracks => {
      this.tracks = tracks;
      this.updateTracks();
      this.trackSortLists$ = this.trackService.getTrackSortLists(this.currentTrackSort);
      this.updateMusicTab(MusicTabEnum.Songs);
    });
  }

  updateMusicTab(musicTab: MusicTabEnum): void {
    this.selectMusicTab = musicTab;

    switch (musicTab) {
      case MusicTabEnum.Songs:
        this.musicCount = this.tracks.length;
        break;
      case MusicTabEnum.Artists:
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
      this.trackSortLists$ = this.trackService.getTrackSortLists(trackSort);
    }
  }

  updateAlbumSort(albumSort: AlbumSortEnum): void {
    if (this.currentAlbumSort !== albumSort) {
      this.currentAlbumSort = albumSort;
      this.albumSortGroups = this.getAlbumSortGroups();
    }
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

  handleScroll(height: number, scrollTop: number): void {
    this.scrollData.top = scrollTop;
    this.scrollData.height = height;

    if (this.scrollData.timeoutId) {
      clearTimeout(this.scrollData.timeoutId);
    }

    this.scrollData.timeoutId = setTimeout((_height, _scrollTop) => this.handleScrollCallback(_height, _scrollTop),
      this.scrollData.timeout, height, scrollTop);
  }

  handleScrollCallback(height: number, scrollTop: number): void {
    const parentTop = scrollTop,
          parentBottom = parentTop + height;

    this.children.reduce((acc, current) => {
      const listTop = acc,
            listBottom = acc + current.list.height,
            viewTop = parentTop - listTop > 0 ? parentTop - listTop : 0,
            viewBottom = viewTop + height;

      if ((listTop >= parentTop && listTop <= parentBottom) ||
          (listBottom >= parentTop && listBottom <= parentBottom) ||
          (parentTop >= listTop && parentTop <= listBottom) ||
          (parentBottom >= listTop && parentBottom <= listBottom)) {
        current.list.showTracks(viewTop, viewBottom);
      } else {
        current.list.hideTracks();
      }

      return listBottom;
    }, 0);
  }
}
