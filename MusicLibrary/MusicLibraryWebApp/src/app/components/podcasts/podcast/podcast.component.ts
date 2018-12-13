import { Component, OnInit, Input } from '@angular/core';
import { Podcast } from 'src/app/shared/models/podcast.model';

@Component({
  selector: 'app-podcast',
  templateUrl: './podcast.component.html',
  styleUrls: ['./podcast.component.css']
})
export class PodcastComponent implements OnInit {
  @Input() podcast: Podcast;

  constructor() { }

  ngOnInit() {
  }

}
