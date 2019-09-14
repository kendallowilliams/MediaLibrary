import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { PodcastItem } from 'src/app/shared/models/podcast-item.model';
import { PodcastService } from 'src/app/services/podcast.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-podcast-items',
  templateUrl: './podcast-items.component.html',
  styleUrls: ['./podcast-items.component.css']
})
export class PodcastItemsComponent implements OnInit, OnDestroy {
  protected items$: Observable<PodcastItem[]>;
  private subscription: Subscription;

  constructor(private route: ActivatedRoute, private podcastService: PodcastService) { }

  ngOnInit() {
    this.subscription = this.route.params.subscribe(params => this.initPodcast(params.podcastId));
  }

  ngOnDestroy(): void {
    this.podcastService.setCurrentPodcastId(0);
    this.subscription.unsubscribe();
  }

  initPodcast(id: number) {
    this.items$ = this.podcastService.getPodcastItems(id);
    this.podcastService.setCurrentPodcastId(id);
  }
}
