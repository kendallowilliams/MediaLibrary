import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Podcast } from '../../../shared/models/podcast.model';
import { Observable } from 'rxjs';
import { PodcastService } from '../../../services/podcast.service';


@Injectable({
    providedIn: 'root'
})

export class PodcastsResolver implements Resolve<Podcast[]> {
    constructor(private podcastService: PodcastService) { }

    resolve(): Observable<Podcast[]> {
        return this.podcastService.getPodcasts();
    }
}
