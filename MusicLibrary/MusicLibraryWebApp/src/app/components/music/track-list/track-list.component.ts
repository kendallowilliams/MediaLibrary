import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER, ViewChildren, QueryList } from '@angular/core';
import { Track } from '../../../shared/models/track.model';
import { ITrackList } from '../../../shared/interfaces/music.interface';
import { TrackComponent } from '../track/track.component';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css']
})
export class TrackListComponent implements OnInit {
  public static HeaderHeight = 30;

  @ViewChildren(TrackComponent) children = new QueryList<TrackComponent>();

  @Input() group: ITrackList;
  @Input() tracks: Track[];

  private tracksHeight: number;
  private headerHeight: number;
  private loaded: boolean;
  private tracksHidden = true;

  constructor() {
  }

  ngOnInit() {
    this.headerHeight = TrackListComponent.HeaderHeight;
    this.tracksHeight = this.group.tracks.length * TrackComponent.TrackHeight;
    this.group.showTracks = () => this.show();
    this.group.hideTracks = () => this.hide();
  }

  load(): void {
    if (!this.loaded) {
      this.tracks = this.group.tracks;
      this.group.height = this.tracksHeight + this.headerHeight;
      this.loaded = true;
    }
  }

  show(): void {
    this.load();
    this.tracksHidden = false;
  }

  hide(): void {
    this.tracksHidden = true;
  }

  trackByTracks(index: number, track: Track): number {
    return track.id;
  }

  playTrack(id: number): void {
    this.children.filter(child => child.track.id !== id).forEach(child => child.isPlaying = false);
  }

  selectTrack(id: number): void {
    this.children.filter(child => child.track.id !== id).forEach(child => child.isSelected = false);
  }

  checkTrack(id: number): void {
    const atLeastOneChildChecked = this.children.some(child => child.isChecked);

    if (atLeastOneChildChecked) {

    }
  }
}
