import { Injectable } from '@angular/core';
import { Track } from '../shared/models/track.model';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class TrackService {
  tracks: Observable<Track[]>;

  constructor(private http: HttpClient) { }

  getTracks(): Observable<Track[]> {
    this.tracks = this.http.get<Track[]>('/api/track');
    return this.tracks;
  }
}
