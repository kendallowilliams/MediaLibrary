import { Injectable } from '@angular/core';
import { Artist } from '../shared/models/artist.model';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ArtistService {
  artists: Artist[];

  constructor() { }

  getArtists(): Observable<Artist[]> {
    return of(this.artists);
  }
}
