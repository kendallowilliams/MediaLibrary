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

  @Input() list: ITrackList;

  private tracksHeight: number;
  private headerHeight: number;
  private loaded: boolean;
  private readonly tracksPerGroup: number = 100;

  constructor(private appService: AppService) {
  }

  ngOnInit() {
    this.headerHeight = TrackListComponent.HeaderHeight;
    this.tracksHeight = this.list.groups.reduce((_previous, _current) =>
      _previous + _current.tracks.length, 0) * TrackComponent.TrackHeight;
    this.list.showTracks = (top, bottom) => this.show(top, bottom);
    this.list.hideTracks = () => this.hide();
  }

  load(): void {
    if (!this.loaded) {
      this.loaded = true;
    }
  }

  show(viewTop: number, viewBottom: number): void {
    this.load();
    this.list.groups.forEach((group, index) => {
      const groupTop: number = index * this.tracksPerGroup * TrackComponent.TrackHeight,
            groupBottom: number = groupTop + (group.tracks.length * TrackComponent.TrackHeight);
      group.visible = (viewTop >= groupTop && viewTop <= groupBottom) ||
        (viewBottom >= groupTop && viewBottom <= groupBottom);
      group.height = group.tracks.length * TrackComponent.TrackHeight;
    });
  }

  hide(): void {
    if (this.loaded) {
      this.list.groups.forEach(group => group.visible = false);
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
