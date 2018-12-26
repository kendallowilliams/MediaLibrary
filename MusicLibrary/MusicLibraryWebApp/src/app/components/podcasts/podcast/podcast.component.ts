import { Component, OnInit, Input } from '@angular/core';
import { Podcast } from 'src/app/shared/models/podcast.model';
import { PodcastItem } from 'src/app/shared/models/podcast-item.model';
import { Observable } from 'rxjs';
import { PodcastService } from 'src/app/services/podcast.service';
import { timeInterval } from 'rxjs/operators';

@Component({
  selector: 'app-podcast',
  templateUrl: './podcast.component.html',
  styleUrls: ['./podcast.component.css']
})
export class PodcastComponent implements OnInit {
  @Input() podcast: Podcast;
  protected items$: Observable<PodcastItem[]>;

  constructor(private podcastService: PodcastService) { }

  ngOnInit() {
    this.items$ = this.podcastService.getPodcastItems(this.podcast.id);
  }

  getNumberOfDays(date: Date): number {
    const MILLISECONDS_PER_DAY = 1000 * 60 * 60 * 24;
    return Math.floor((Date.now() - date.getTime()) / MILLISECONDS_PER_DAY);
  }
}
