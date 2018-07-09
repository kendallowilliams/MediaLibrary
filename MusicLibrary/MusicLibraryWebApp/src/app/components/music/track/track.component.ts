import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Track } from '../../../shared/models/track.model';

@Component({
  selector: 'app-track',
  templateUrl: './track.component.html',
  styleUrls: ['./track.component.css']
})

export class TrackComponent implements OnInit {
  public static TrackHeight = 40;

  @Input() track: Track;

  @Output() select: EventEmitter<number> = new EventEmitter<number>();
  @Output() check: EventEmitter<number> = new EventEmitter<number>();
  @Output() play: EventEmitter<number> = new EventEmitter<number>();

  private _isSelected: boolean;
  private _isChecked: boolean;
  private _isPlaying: boolean;

  get isSelected() {
    return this._isSelected;
  }

  set isSelected(value: boolean) {
    this._isSelected = value;
  }

  get isChecked() {
    return this._isChecked;
  }

  set isChecked(value: boolean) {
    this._isChecked = value;
  }

  get isPlaying() {
    return this._isPlaying;
  }

  set isPlaying(value: boolean) {
    this._isPlaying = value;
  }

  constructor() { }

  ngOnInit() {
  }

  getTrackHeight(): number {
    return TrackComponent.TrackHeight;
  }

  selectTrack(): void {
    this.isSelected = !this.isSelected;
    this.select.emit(this.track.id);
  }

  checkTrack(evt: any): void {
    this.isChecked = !this.isChecked;
    this.check.emit(this.track.id);
  }

  playTrack(): void {
    this.isPlaying = !this.isPlaying;
    this.play.emit(this.track.id);
  }
}
