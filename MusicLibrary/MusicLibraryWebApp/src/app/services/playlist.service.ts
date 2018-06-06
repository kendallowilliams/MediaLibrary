import { Injectable } from '@angular/core';
import { Playlist } from '../shared/models/playlist.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class PlaylistService {
  playlists: Playlist[];

  constructor(private http: HttpClient) { }

  getPlaylists(): Observable<Playlist[]> {
    return this.http.get<Playlist[]>('/api/Playlist')
                    .pipe(map(playlists => playlists.map(playlist => new Playlist().deserialize(playlist))));
  }
}
