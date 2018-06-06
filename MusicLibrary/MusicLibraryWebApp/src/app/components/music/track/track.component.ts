import { Component, OnInit, Input } from '@angular/core';
import { Track } from '../../../shared/models/track.model';

@Component({
  selector: 'app-track',
  templateUrl: './track.component.html',
  styleUrls: ['./track.component.css']
})

export class TrackComponent implements OnInit {
  @Input() track: Track;

  constructor() { }

  ngOnInit() {
  }

}
