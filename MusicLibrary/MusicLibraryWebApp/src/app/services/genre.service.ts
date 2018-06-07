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

  getGenres(): Observable<Genre[]> {
    return this.http.get<Genre[]>('/api/Genre')
                    .pipe(map(genres => genres.map(genre => new Genre().deserialize(genre))));
  }
}
