import { Injectable } from '@angular/core';
import { Artist } from '../shared/models/artist.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { IArtistList } from '../shared/interfaces/music.interface';

@Injectable({
  providedIn: 'root'
})

export class ArtistService {
  private letters: Array<string> = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  artists: Artist[];

  constructor(private http: HttpClient) { }

  getArtists(): Observable<Artist[]> {
    return this.http.get<Artist[]>('/api/Artist')
                    .pipe(map(artists => artists.map(artist => new Artist().deserialize(artist))),
                          map(artists => this.artists = artists));
  }

  getArtist(id: number): Observable<Artist> {
    const artistId: number = !!id ? id : -1;
    let artist: Observable<Artist>;

    if (!!this.artists) {
      artist = of(this.artists.find(_artist => _artist.id === artistId));
    } else {
      artist = this.http.get<Artist>('/api/Artist/' + artistId)
                        .pipe(map(_artist => new Artist().deserialize(_artist)));
    }

    return artist;
  }

  getArtistSortGroups(): Observable<IArtistList[]> {
    let lists: Observable<IArtistList[]> = of();
    const artists: Observable<Artist[]> = this.getArtists();

    lists = artists.pipe(map((_artists, _index) =>
      ['&', '#'].concat(this.letters).map(char => ({
        title: char,
        artists: this.getArtistsAtoZ(char, _artists)
      })
    )));

    return lists;
  }

  getArtistsAtoZ(char: string, artists: Artist[]): Artist[] {
    let _artists: Artist[] = [];

    switch (char) {
      case '&':
        _artists = artists.filter(artist => isNaN(parseInt(artist.name[0], 10)) &&
          !this.letters.includes(artist.name[0].toUpperCase()));
        break;
      case '#':
        _artists = artists.filter(artist => !isNaN(parseInt(artist.name[0], 10)));
        break;
      default:
        _artists = artists.filter(artist => artist.name[0].toUpperCase() === char);
        break;
    }

    return _artists;
  }
}
