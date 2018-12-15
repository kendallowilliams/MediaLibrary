import { Component, OnInit, Input } from '@angular/core';
import { Podcast } from 'src/app/shared/models/podcast.model';
import { PodcastItem } from 'src/app/shared/models/podcast-item.model';
import { Observable } from 'rxjs';
import { PodcastService } from 'src/app/services/podcast.service';

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

}
