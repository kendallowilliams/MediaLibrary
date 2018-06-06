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
  tracks: Observable<Track[]>;

  constructor(private trackService: TrackService) { }

  ngOnInit() {
    this.tracks = this.trackService.getTracks();
  }

}
