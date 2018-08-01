import { Injectable } from '@angular/core';
import { Artist } from '../shared/models/artist.model';
import { Observable } from 'rxjs';
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
                    .pipe(map(artists => artists.map(artist => new Artist().deserialize(artist))));
  }

  getArtist(id: number): Observable<Artist> {
    return this.http.get<Artist>('/api/Artist/' + id)
                    .pipe(map(artist => new Artist().deserialize(artist)));
  }
}
