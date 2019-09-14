import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Playlist } from '../../../shared/models/playlist.model';
import { Observable } from 'rxjs';
import { PlaylistService } from '../../../services/playlist.service';


@Injectable({
    providedIn: 'root'
})

export class PlaylistsResolver implements Resolve<Playlist[]> {
    constructor(private playlistService: PlaylistService) { }

    resolve(): Observable<Playlist[]> {
        return this.playlistService.getPlaylists();
    }
}
