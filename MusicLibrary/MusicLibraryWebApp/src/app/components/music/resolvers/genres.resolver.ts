import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Genre } from '../../../shared/models/genre.model';
import { Observable } from 'rxjs';
import { GenreService } from '../../../services/genre.service';


@Injectable({
    providedIn: 'root'
})

export class GenresResolver implements Resolve<Genre[]> {
    constructor(private genreService: GenreService) { }

    resolve(): Observable<Genre[]> {
        return this.genreService.getGenres();
    }
}
