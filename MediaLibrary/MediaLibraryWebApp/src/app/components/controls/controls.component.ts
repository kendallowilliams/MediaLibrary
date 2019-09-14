import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NowPlayingService } from '../../services/now-playing.service';
import { AppService } from 'src/app/services/app.service';
import { ReadyStateEnum } from '../music/enums/player-enum';
import { nextTick } from 'q';

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
  private playerTimeout: number;

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
    // this.player.onplay = evt => this.isPlaying = true;
    // this.player.onpause = evt => this.isPlaying = false;
    this.player.onended = evt => this.next();
    this.player.onerror = evt => this.handlePlayerError(evt as Event);
    this.nextPreviousClicked = this.isRepeatOne = this.isRepeatAll = this.isPlaying = false;
  }

  play(): void {
    if (this.player.readyState !== ReadyStateEnum.HAVE_NOTHING) {
      this.player.play();
      this.isPlaying = true;
    }
    this.playerTimeout = null;
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
      if (!this.nextPreviousClicked || this.isPlaying) {
        if (!!this.playerTimeout) {
          clearTimeout(this.playerTimeout);
        }
        this.playerTimeout = setTimeout.call(this, () => this.play(), 1000);
      }
    }
    this.nextPreviousClicked = false;
  }

  handlePlayerError(evt: Event): void {
    console.log(this.player.error.code + ' ' + this.player.error['message']);
    if (this.isPlaying) {
      this.next();
    }
  }
}
