import { Injectable } from '@angular/core';
import { Podcast } from '../shared/models/podcast.model';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, } from '@angular/common/http';
import { PodcastItem } from '../shared/models/podcast-item.model';

@Injectable({
  providedIn: 'root'
})

export class PodcastService {
  private podcastSubject: BehaviorSubject<number>;

  constructor(private http: HttpClient) {
    this.podcastSubject = new BehaviorSubject<number>(0);
  }

  getPodcasts(): Observable<Podcast[]> {
    return this.http.get<Podcast[]>('/api/Podcast')
                    .pipe(map(podcasts => podcasts.map(podcast => new Podcast().deserialize(podcast))));
  }

  getPodcast(id: number): Observable<Podcast> {
    return this.http.get<Podcast>('/api/Podcast/' + (id || 0))
                    .pipe(map(podcast => new Podcast().deserialize(podcast)));
  }

  getPodcastItems(id: number): Observable<PodcastItem[]> {
    return this.http.get<Podcast[]>('/api/Podcast/GetPodcastItems/' + (id || 0))
                    .pipe(map(items => items.map(item => new PodcastItem().deserialize(item))));
  }

  deletePodcast(id: number): Observable<boolean> {
    return this.http.delete('/api/Podcast/' + (id || 0)).pipe(map(result => result as boolean));
  }

  refreshPodcast(podcast: Podcast): Observable<Podcast> {
    return this.http.post('/api/Podcast/Refresh', podcast).pipe(map(_podcast => new Podcast().deserialize(_podcast)));
  }

  setCurrentPodcastId(id: number) {
    this.podcastSubject.next(id);
  }

  getCurrentPodcastId(): Observable<number> {
    return this.podcastSubject.asObservable();
  }
}
