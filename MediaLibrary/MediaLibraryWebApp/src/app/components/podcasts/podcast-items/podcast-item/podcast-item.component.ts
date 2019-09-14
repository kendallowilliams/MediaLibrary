import { Component, OnInit, Input } from '@angular/core';
import { PodcastItem } from 'src/app/shared/models/podcast-item.model';

@Component({
  selector: 'app-podcast-item',
  templateUrl: './podcast-item.component.html',
  styleUrls: ['./podcast-item.component.css']
})
export class PodcastItemComponent implements OnInit {
  @Input() item: PodcastItem;

  constructor() { }

  ngOnInit() {
  }

}
