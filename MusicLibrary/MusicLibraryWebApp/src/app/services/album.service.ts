import { Injectable } from '@angular/core';
import { Album } from '../shared/models/album.model';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AlbumService {
  albums: Album[];

  constructor() { }

  getAlbums(): Observable<Album[]> {
    return of(this.albums);
  }
}
