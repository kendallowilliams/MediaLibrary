import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Track } from '../../../shared/models/track.model';
import { Observable } from 'rxjs';
import { TrackService } from '../../../services/track.service';


@Injectable({
    providedIn: 'root'
})

export class TracksResolver implements Resolve<Track[]> {
    constructor(private trackService: TrackService) { }

    resolve(): Observable<Track[]> {
        return this.trackService.getTracks();
    }
}
