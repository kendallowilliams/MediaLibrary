import { Component, OnInit, Output, EventEmitter, Input, ViewChild, ElementRef } from '@angular/core';
import { NowPlayingService } from '../../services/now-playing.service';
import { AppService } from 'src/app/services/app.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.component.html',
  styleUrls: ['./controls.component.css']
})
export class ControlsComponent implements OnInit {
  @ViewChild('player') playerRef: ElementRef<HTMLAudioElement>;
  private player: HTMLAudioElement;

  constructor(private nowPlayingService: NowPlayingService, private appService: AppService) {
    this.appService.controlsComponent = this;
  }

  ngOnInit() {
    this.player = this.playerRef.nativeElement;
    this.nowPlayingService.getCurrentTrackId().subscribe(id => {
      if (!!id && id > 0) {
        this.player.src = 'api/Track/GetTrackFile/' + id;
      }
    });
  }

  play(): void {
    if (this.player.paused) {
      this.player.play();
    } else {
      this.player.pause();
    }
  }

  playNext(): void {
    this.nowPlayingService.setNextTrackId();
  }

  playPrevious(): void {
    this.nowPlayingService.setPreviousTrackId();
  }
}
