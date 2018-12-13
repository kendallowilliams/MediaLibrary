import { Component, OnInit } from '@angular/core';
import { PodcastService } from 'src/app/services/podcast.service';
import { Observable } from 'rxjs';
import { Podcast } from 'src/app/shared/models/podcast.model';

@Component({
  selector: 'app-podcasts',
  templateUrl: './podcasts.component.html',
  styleUrls: ['./podcasts.component.css']
})
export class PodcastsComponent implements OnInit {
  protected podcasts$: Observable<Podcast[]>;

  constructor(private podcastService: PodcastService) { }

  ngOnInit() {
    this.podcasts$ = this.podcastService.getPodcasts();
  }

}
