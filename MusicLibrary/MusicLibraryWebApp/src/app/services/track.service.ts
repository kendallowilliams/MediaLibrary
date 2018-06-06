import { Injectable } from '@angular/core';
import { Track } from '../shared/models/track.model'
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class TrackService {
  tracks: Track[];

  constructor() { }

  getTracks(): Observable<Track[]> {
    return of(this.tracks);
  }
}
