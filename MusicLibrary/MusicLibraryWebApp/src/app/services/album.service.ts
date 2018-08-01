import { Injectable } from '@angular/core';
import { Album } from '../shared/models/album.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class AlbumService {
  albums: Album[];

  constructor(private http: HttpClient) { }

  getAlbums(): Observable<Album[]> {
    return this.http.get<Album[]>('/api/Album')
                    .pipe(map(albums => albums.map(album => new Album().deserialize(album))));
  }

  getAlbum(id: number): Observable<Album> {
    return this.http.get<Album>('/api/Album/' + id)
                    .pipe(map(album => new Album().deserialize(album)));
  }
}
