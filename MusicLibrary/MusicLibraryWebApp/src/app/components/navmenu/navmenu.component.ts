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

  constructor(private route: ActivatedRoute, private podcastService: PodcastService) {
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => this.queryParamsChanged(params));
  }

  queryParamsChanged(params: Params) {
    if (!!params) {
      this.podcast$ = !!(params.podcastId) ? this.podcastService.getPodcast(params.podcastId) : null;
    }
  }
}
