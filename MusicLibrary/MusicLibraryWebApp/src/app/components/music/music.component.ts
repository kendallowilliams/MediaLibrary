import { Component, OnInit } from '@angular/core';
import { TrackService } from '../../services/track.service';
import { Track } from '../../shared/models/track.model';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  tracks: Track[];

  constructor(private trackService: TrackService) { }

  ngOnInit() {
    this.getTracks();
  }

  getTracks(): void {
    this.trackService.getTracks().subscribe(tracks => this.tracks = tracks);
  }
}
