import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER, ViewChildren, QueryList } from '@angular/core';
import { Track } from '../../../shared/models/track.model';
import { ITrackList, ITrackGroup } from '../../../shared/interfaces/music.interface';
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
  private readonly tracksPerGroup: number = 100;
  private trackGroups: ITrackGroup[];

  constructor(private appService: AppService) {
  }

  ngOnInit() {
    this.headerHeight = TrackListComponent.HeaderHeight;
    this.tracksHeight = this.group.tracks.length * TrackComponent.TrackHeight;
    this.group.showTracks = (top, bottom) => this.show(top, bottom);
    this.group.hideTracks = () => this.hide();
  }

  load(): void {
    if (!this.loaded) {
      this.trackGroups = this.splitTracksIntoGroups(this.group.tracks);
      this.group.height = this.tracksHeight + this.headerHeight;
      this.loaded = true;
    }
  }

  splitTracksIntoGroups(_tracks: Track[]): ITrackGroup[] {
    const groupCount: number = Math.ceil(_tracks.length / this.tracksPerGroup);
    const trackGroups: ITrackGroup[] = [];
    const tracks = Array.from(_tracks);

    while (tracks.length > 0) {
      trackGroups.push({ tracks: tracks.splice(0, this.tracksPerGroup) });
    }

    return trackGroups;
  }

  show(viewTop: number, viewBottom: number): void {
    this.load();
    this.trackGroups.forEach((group, index) => {
      const groupTop: number = index * this.tracksPerGroup * TrackComponent.TrackHeight,
            groupBottom: number = groupTop + (group.tracks.length * TrackComponent.TrackHeight);
      group.visible = (viewTop >= groupTop && viewTop <= groupBottom) ||
        (viewBottom >= groupTop && viewBottom <= groupBottom);
      group.height = group.tracks.length * TrackComponent.TrackHeight;
    });
  }

  hide(): void {
    if (this.loaded) {
      this.trackGroups.forEach(group => group.visible = false);
    }
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
