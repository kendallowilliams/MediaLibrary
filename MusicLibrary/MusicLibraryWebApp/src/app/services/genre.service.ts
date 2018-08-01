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
                    .pipe(map(genres => genres.map(genre => new Genre().deserialize(genre))),
                          map(genres => this.genres = genres));
  }

  getGenre(id: number): Observable<Genre> {
    const genreId: number = !!id ? id : -1;
    let genre: Observable<Genre>;

    if (!!this.genres) {
      genre = of(this.genres.find(_genre => _genre.id === genreId));
    } else {
      genre = this.http.get<Genre>('/api/Genre/' + id)
                    .pipe(map(_genre => new Genre().deserialize(_genre)));
    }

    return genre;
  }
}
