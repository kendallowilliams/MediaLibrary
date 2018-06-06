import { Injectable } from '@angular/core';
import { Podcast } from '../shared/models/podcast.model';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class PodcastService {
  podcasts: Podcast[];

  constructor() { }

  getPodcasts(): Observable<Podcast[]> {
    return of(this.podcasts);
  }
}
