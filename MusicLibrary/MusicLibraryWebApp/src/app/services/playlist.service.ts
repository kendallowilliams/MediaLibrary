import { Injectable } from '@angular/core';
import { Playlist } from '../shared/models/playlist.model';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlaylistService {
  playlists: Playlist[];

  constructor() { }

  getPlaylists(): Observable<Playlist[]> {
    return of(this.playlists);
  }
}
