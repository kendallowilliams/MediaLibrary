import { Injectable } from '@angular/core';
import { Genre } from '../shared/models/genre.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  genres: Genre[];

  constructor(private http: HttpClient) { }

  getPlaylists(): Observable<Genre[]> {
    return this.http.get<Genre[]>('/api/Playlist')
                    .pipe(map(genres => genres.map(genre => new Genre().deserialize(genre))));
  }
}
