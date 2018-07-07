import { Component, OnInit, Input } from '@angular/core';

import { Track } from '../../../shared/models/track.model';

@Component({
  selector: 'app-track',
  templateUrl: './track.component.html',
  styleUrls: ['./track.component.css']
})

export class TrackComponent implements OnInit {
  public static TrackHeight = 40;

  @Input() track: Track;

  private hidden = true;

  constructor() { }

  ngOnInit() {
  }

  getTrackHeight(): number {
    return TrackComponent.TrackHeight;
  }
}
