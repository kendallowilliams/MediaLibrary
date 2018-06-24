import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER } from '@angular/core';
import { Track } from '../../../shared/models/track.model';
import { ITrackList } from '../../../shared/interfaces/music.interface';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css']
})
export class TrackListComponent implements OnInit {
  @Input() group: ITrackList;
  @Input() tracks: Track[];

  private height: number;
  private top: number;


  constructor() {
  }

  ngOnInit() {
    this.group.loadCallback = () => this.load();
  }

  load(): void {
    this.tracks = this.group.tracks;
  }

  trackByTracks(index: number, track: Track): number {
    return track.id;
  }
}
