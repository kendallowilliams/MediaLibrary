import { Injectable } from '@angular/core';
import { Playlist } from '../shared/models/playlist.model';
import { Observable, of } from 'rxjs';
import {Http, Response} from '@angular/http';

@Injectable({
  providedIn: 'root'
})

export class PlaylistService {
  playlists: Playlist[];

  constructor(private http: Http) { }

  getPlaylists(): Observable<Playlist[]> {
    return of(this.playlists);
  }
}
