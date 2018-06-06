import { Injectable } from '@angular/core';
import { Album } from '../shared/models/album.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders} from '@angular/common/http';

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
}
