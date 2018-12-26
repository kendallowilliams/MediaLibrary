import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { PodcastService } from 'src/app/services/podcast.service';
import { Podcast } from 'src/app/shared/models/podcast.model';

@Component({
  selector: 'app-navmenu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.css']
})
export class NavmenuComponent implements OnInit {
  protected podcast$: Observable<Podcast>;

  constructor(private podcastService: PodcastService) {
  }

  ngOnInit() {
    this.podcastService.getCurrentPodcastId().subscribe(id => this.podcastIdChanged(id));
  }

  podcastIdChanged(id: number) {
    this.podcast$ = !!id ? this.podcastService.getPodcast(id) : null;
  }
}
