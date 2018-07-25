import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AppService } from '../../services/app.service';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.component.html',
  styleUrls: ['./controls.component.css']
})
export class ControlsComponent implements OnInit {
  @Input() audioSrc: string;

  constructor(private appService: AppService) {
    this.appService.controlsComponent = this;
  }

  ngOnInit() {
  }

  play(trackId: number): void {
    this.audioSrc = 'api/Track/GetTrackFile/' + trackId;
  }
}
