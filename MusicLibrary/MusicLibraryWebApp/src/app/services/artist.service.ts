import { Injectable } from '@angular/core';
import { Artist } from '../shared/models/artist.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class ArtistService {
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
}
