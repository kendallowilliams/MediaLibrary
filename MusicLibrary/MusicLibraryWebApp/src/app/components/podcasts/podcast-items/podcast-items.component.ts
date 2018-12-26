import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { PodcastItem } from 'src/app/shared/models/podcast-item.model';
import { PodcastService } from 'src/app/services/podcast.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-podcast-items',
  templateUrl: './podcast-items.component.html',
  styleUrls: ['./podcast-items.component.css']
})
export class PodcastItemsComponent implements OnInit {
  private podcastId: number;
  protected items$: Observable<PodcastItem[]>;

  constructor(private route: ActivatedRoute, private podcastService: PodcastService) { }

  ngOnInit() {
    this.route.params.subscribe(params => this.initPodcast(params.podcastId));
  }

  initPodcast(id: number) {
    this.items$ = this.podcastService.getPodcastItems(id);
    this.podcastService.setCurrentPodcastId(id);
  }
}
