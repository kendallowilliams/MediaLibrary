import { Component, OnInit, Input, ElementRef, Output } from '@angular/core';
import { Track } from '../../../shared/models/track.model';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css']
})
export class TrackListComponent implements OnInit {
  @Input() group: any;
  @Output() tracks: Track[];

   private height: number;
   private top: number;


  constructor(private el: ElementRef) {
  }

  ngOnInit() {
  }

  trackByTracks(index: number, track: Track): number {
    return track.id;
  }
}
