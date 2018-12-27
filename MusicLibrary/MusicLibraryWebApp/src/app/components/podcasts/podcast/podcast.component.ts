import { Component, OnInit, Input} from '@angular/core';
import { Podcast } from 'src/app/shared/models/podcast.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-podcast',
  templateUrl: './podcast.component.html',
  styleUrls: ['./podcast.component.css']
})
export class PodcastComponent implements OnInit {
  @Input() podcast: Podcast;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  getNumberOfDays(date: Date): number {
    const MILLISECONDS_PER_DAY = 1000 * 60 * 60 * 24;
    return Math.floor((Date.now() - date.getTime()) / MILLISECONDS_PER_DAY);
  }

  podcastClicked(id: number) {
    this.router.navigate(['/podcasts', id]);
  }
}
