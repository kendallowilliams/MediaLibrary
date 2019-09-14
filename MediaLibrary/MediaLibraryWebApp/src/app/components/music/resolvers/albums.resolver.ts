import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Album } from '../../../shared/models/album.model';
import { Observable } from 'rxjs';
import { AlbumService } from '../../../services/album.service';


@Injectable({
    providedIn: 'root'
})

export class AlbumsResolver implements Resolve<Album[]> {
    constructor(private albumService: AlbumService) { }

    resolve(): Observable<Album[]> {
        return this.albumService.getAlbums();
    }
}
