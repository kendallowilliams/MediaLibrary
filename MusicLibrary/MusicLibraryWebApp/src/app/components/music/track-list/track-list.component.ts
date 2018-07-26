import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER, ViewChildren, QueryList } from '@angular/core';
import { Track } from '../../../shared/models/track.model';
import { ITrackList } from '../../../shared/interfaces/music.interface';
import { TrackComponent } from '../track/track.component';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css']
})
export class TrackListComponent implements OnInit {
  public static HeaderHeight = 30;

  @ViewChildren(TrackComponent) children = new QueryList<TrackComponent>();

  @Input() group: ITrackList;

  private tracksHeight: number;
  private headerHeight: number;
  private loaded: boolean;
  private tracksHidden = true;
  private readonly tracksPerGroup: number = 100;
  private trackGroups: Array<Track[]>;

  constructor(private appService: AppService) {
  }

  ngOnInit() {
    this.headerHeight = TrackListComponent.HeaderHeight;
    this.tracksHeight = this.group.tracks.length * TrackComponent.TrackHeight;
    this.group.showTracks = () => this.show();
    this.group.hideTracks = () => this.hide();
  }

  load(): void {
    if (!this.loaded) {
      this.trackGroups = this.splitTracksIntoGroups(this.group.tracks);
      this.group.height = this.tracksHeight + this.headerHeight;
      this.loaded = true;
    }
  }

  splitTracksIntoGroups(_tracks: Track[]): Array<Track[]> {
    const trackGroups: Array<Track[]> = [];
    const tracks = Array.from(_tracks);

    while (tracks.length > 0) {
      trackGroups.push(tracks.splice(0, this.tracksPerGroup));
    }

    return trackGroups;
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
    this.appService.controlsComponent.play(id);
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
