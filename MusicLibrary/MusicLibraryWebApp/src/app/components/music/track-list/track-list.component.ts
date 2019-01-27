import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER, ViewChildren, QueryList } from '@angular/core';
import { Track } from '../../../shared/models/track.model';
import { ITrackList, ITrackGroup } from '../../../shared/interfaces/music.interface';
import { TrackRowComponent } from './track-row/track-row.component';
import { AppService } from '../../../services/app.service';
import { NowPlayingService } from 'src/app/services/now-playing.service';
import { Guid } from 'guid-typescript';
import { Observable } from 'rxjs';
import { IListItem } from 'src/app/shared/interfaces/list-item.interface';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css']
})
export class TrackListComponent implements OnInit {
  public static HeaderHeight = 30;

  @ViewChildren(TrackRowComponent) children = new QueryList<TrackRowComponent>();
  @Input() list: ITrackList;

  protected tracksHeight: number;
  protected headerHeight: number;
  private loaded: boolean;
  private readonly tracksPerGroup: number = 100;

  constructor(private appService: AppService, private nowPlayingService: NowPlayingService) {
  }

  ngOnInit() {
    this.headerHeight = TrackListComponent.HeaderHeight;
    this.tracksHeight = this.list.groups.reduce((_previous, _current) =>
      _previous + _current.tracks.length, 0) * TrackRowComponent.TrackHeight;
    this.list.showTracks = (top, bottom) => this.show(top, bottom);
    this.list.hideTracks = () => this.hide();
    this.nowPlayingService.getCurrentTrackId().subscribe(item => {
      if (!!item) {
        this.children.forEach(child => child.isPlaying = child.track.id === item.value);
      }
    });
  }

  load(): void {
    if (!this.loaded) {
      this.loaded = true;
    }
  }

  show(viewTop: number, viewBottom: number): void {
    this.load();
    this.list.groups.forEach((group, index) => {
      const groupTop: number = index * this.tracksPerGroup * TrackRowComponent.TrackHeight,
            groupBottom: number = groupTop + (group.tracks.length * TrackRowComponent.TrackHeight);
      group.visible = (viewTop >= groupTop && viewTop <= groupBottom) ||
        (viewBottom >= groupTop && viewBottom <= groupBottom);
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

  playTrack(trackId: number): void {
    const item: IListItem = { id: null, value: trackId };

    this.nowPlayingService.setCurrentTrackId(item);
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
