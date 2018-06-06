import { Injectable } from '@angular/core';
import { Track } from '../shared/models/track.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class TrackService {
  constructor(private http: HttpClient) { }

  getTracks(): Observable<Track[]> {
    return this.http.get<Track[]>('/api/Track')
                    .pipe(map(tracks => tracks.map(track => new Track().deserialize(track))));
  }
}
