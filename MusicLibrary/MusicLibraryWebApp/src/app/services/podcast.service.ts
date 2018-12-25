import { Injectable } from '@angular/core';
import { Podcast } from '../shared/models/podcast.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { PodcastItem } from '../shared/models/podcast-item.model';

@Injectable({
  providedIn: 'root'
})

export class PodcastService {
  podcasts: Podcast[];

  constructor(private http: HttpClient) { }

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
}
