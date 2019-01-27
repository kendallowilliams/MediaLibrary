import { Component, OnInit, Output, EventEmitter, Input, ViewChild, ElementRef } from '@angular/core';
import { NowPlayingService } from '../../services/now-playing.service';
import { AppService } from 'src/app/services/app.service';
import { ReadyStateEnum } from '../music/enums/player-enum';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.component.html',
  styleUrls: ['./controls.component.css']
})
export class ControlsComponent implements OnInit {
  @ViewChild('player') playerRef: ElementRef<HTMLAudioElement>;
  private player: HTMLAudioElement;
  private isPlaying: boolean;
  private nextPreviousClicked: boolean;
  private isRepeatAll: boolean;
  private isRepeatOne: boolean;

  constructor(private nowPlayingService: NowPlayingService, private appService: AppService) {
    this.appService.controlsComponent = this;
  }

  ngOnInit() {
    this.player = this.playerRef.nativeElement;
    this.nowPlayingService.getCurrentTrackId().subscribe(item => {
      if (!!item) {
        this.loadPlayer(item.value);
      }
    });
    this.player.onplay = evt => this.isPlaying = true;
    // this.player.onpause = evt => this.isPlaying = false;
    this.player.onended = evt => this.next();
  }

  play(): void {
    if (this.player.readyState !== ReadyStateEnum.HAVE_NOTHING) {
      this.player.play();
    }
  }

  pause(): void {
    this.player.pause();
    this.isPlaying = false;
  }

  next(): void {
    const item = this.nowPlayingService.getNextTrackId(this.isRepeatAll);
    this.nextPreviousClicked = true;
    this.nowPlayingService.setCurrentTrackId(item);
  }

  previous(): void {
    const item = this.nowPlayingService.getPreviousTrackId();
    this.nextPreviousClicked = true;
    this.nowPlayingService.setCurrentTrackId(item);
  }

  loadPlayer(id: number): void {
    if (!!id && id > 0) {
      this.player.src = 'api/Track/GetTrackFile/' + id;
      if (!this.nextPreviousClicked || this.isPlaying) { this.player.play(); }
      this.nextPreviousClicked = false;
    }
  }
}
