import { Injectable } from '@angular/core';
import { Album } from '../shared/models/album.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { IAlbumList } from '../shared/interfaces/music.interface';
import { AlbumSortEnum } from '../components/music/enums/music-enum';
import { ArtistService } from './artist.service';
import { GenreService } from './genre.service';
import { IAlbum } from '../shared/interfaces/album.interface';

@Injectable({
  providedIn: 'root'
})

export class AlbumService {
  private letters: Array<string> = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  private albums: Album[] = [];

  constructor(private http: HttpClient, private artistService: ArtistService, private genreService: GenreService) {
  }

  getAlbums(): Observable<Album[]> {
    return this.http.get<IAlbum[]>('/api/Album')
      .pipe(map(albums => albums.map(album => new Album().deserialize(album))),
        map(albums => this.albums = albums));
  }

  getAlbum(id: number): Observable<Album> {
    const albumId: number = !!id ? id : -1;
    let album: Observable<Album>;

    if (!!this.albums && this.albums.length > 0) {
      album = of(this.albums.find(_album => _album.id === albumId));
    } else {
      album = this.http.get<IAlbum>('/api/Album/' + id)
        .pipe(map(_album => new Album().deserialize(_album)));
    }

    return album;
  }

  getAlbumSortLists(albumSort: AlbumSortEnum): Observable<IAlbumList[]> {
    let lists: Observable<IAlbumList[]> = of();
    const albums: Observable<Album[]> = this.getAlbums();

    lists = albums.pipe(map((_albums, _index) => {
      let _lists: IAlbumList[] = [];

      this.updateAlbums(_albums);

      switch (albumSort) {
        case AlbumSortEnum.Artist:
          _lists = this.getArtistIds(_albums).map(artistId => ({
              title: this.artistService.getArtist(artistId).pipe(map(artist => artist.name)),
              albums: this.albums.filter(album => album.artistId === artistId)
            }));
          break;
        case AlbumSortEnum.AtoZ:
          _lists = ['&', '#'].concat(this.letters)
            .map(char => ({
              title: of(char),
              albums: this.getAlbumsAtoZ(char, _albums)
            }));
          break;
        case AlbumSortEnum.DateAdded:
          const dates = _albums.map(album => album.createDate.toDateString()).sort().reverse();
          _lists = dates.filter((date, index) => dates.findIndex(item => item === date) === index).map(date => ({
              title: of(date.toString()),
              albums: this.albums.filter(album => album.createDate.toDateString() === date)
            }));
          break;
        case AlbumSortEnum.ReleaseYear:
          const years = _albums.map(album => album.year).sort().reverse();
          _lists = years.filter((year, index) => years.findIndex(item => item === year) === index).map(year => ({
              title: of(year === 0 ? 'unknown' : year.toString()),
              albums: this.albums.filter(album => album.year === year)
            }));
          break;
        case AlbumSortEnum.None:
        default:
          _lists = [];
          break;
      }

      return _lists;
    }));

    return lists.pipe(map(_lists => _lists.filter(list => list.albums.length > 0)));
  }

  getAlbumsAtoZ(char: string, albums: Album[]): Album[] {
    let _albums = [];

    switch (char) {
      case '&':
        _albums = albums.filter(album => isNaN(parseInt(album.title[0], 10)) &&
          !this.letters.includes(album.title[0].toUpperCase()));
        break;
      case '#':
        _albums = albums.filter(album => !isNaN(parseInt(album.title[0], 10)));
        break;
      default:
        _albums = albums.filter(album => album.title[0].toUpperCase() === char);
        break;
    }

    return _albums;
  }

  getArtistIds(albums: Album[]): number[] {
    return albums.map(album => album.artistId)
      .filter((artistId, index, _artistIds) => _artistIds.findIndex(item => item === artistId) === index);
  }

  updateAlbums(albums: Album[]): void {
    albums.forEach(album => {
      album.artist = this.artistService.getArtist(album.artistId).pipe(map(artist => artist.name));
      album.genre = this.genreService.getGenre(album.genreId).pipe(map(genre => genre.name));
    });
  }
}
