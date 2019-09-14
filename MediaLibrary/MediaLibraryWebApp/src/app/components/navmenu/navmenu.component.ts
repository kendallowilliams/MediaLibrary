import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { PodcastService } from 'src/app/services/podcast.service';
import { Podcast } from 'src/app/shared/models/podcast.model';

@Component({
  selector: 'app-navmenu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.css']
})
export class NavmenuComponent implements OnInit, OnDestroy {
  protected podcast$: Observable<Podcast>;
  private podcastSub: Subscription;

  constructor(private podcastService: PodcastService) {
  }

  ngOnInit() {
    this.podcastSub = this.podcastService.getCurrentPodcastId().subscribe(id => this.podcastIdChanged(id));
  }

  ngOnDestroy(): void {
    this.podcastSub.unsubscribe();
  }

  podcastIdChanged(id: number) {
    this.podcast$ = !!id ? this.podcastService.getPodcast(id) : null;
  }
}
