import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Artist } from '../../../shared/models/artist.model';
import { Observable } from 'rxjs';
import { ArtistService } from '../../../services/artist.service';


@Injectable({
    providedIn: 'root'
})

export class ArtistsResolver implements Resolve<Artist[]> {
    constructor(private artistService: ArtistService) { }

    resolve(): Observable<Artist[]> {
        return this.artistService.getArtists();
    }
}
