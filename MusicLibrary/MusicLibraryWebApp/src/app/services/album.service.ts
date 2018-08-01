import { Injectable } from '@angular/core';
import { Album } from '../shared/models/album.model';
import { Observable, of } from 'rxjs';
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
                    .pipe(map(albums => albums.map(album => new Album().deserialize(album))),
                          map(albums => this.albums = albums));
  }

  getAlbum(id: number): Observable<Album> {
    const albumId: number = !!id ? id : -1;
    let album: Observable<Album>;

    if (!!this.albums) {
      album = of(this.albums.find(_album => _album.id === albumId));
    } else {
      album = this.http.get<Album>('/api/Album/' + id)
                       .pipe(map(_album => new Album().deserialize(_album)));
    }

    return album;
  }
}
