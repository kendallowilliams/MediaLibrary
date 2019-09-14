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

  getLastUpdateDisplay(date: Date): string {
    const MILLISECONDS_PER_DAY = 1000 * 60 * 60 * 24,
          dayCount = Math.floor((Date.now() - date.getTime()) / MILLISECONDS_PER_DAY),
          monthCount = Math.floor(dayCount / 30),
          yearCount = Math.floor(monthCount / 12);
    let output = '';

    if (yearCount > 0) {
      output = yearCount + (yearCount > 1 ? ' years' : ' year');
    } else if (monthCount > 0) {
      output = monthCount + (monthCount > 1 ? ' months' : ' month');
    } else {
      output = dayCount + (dayCount > 1 ? ' days' : ' day');
    }

    return output;
  }

  podcastClicked(id: number) {
    this.router.navigate(['/podcasts', id]);
  }
}
