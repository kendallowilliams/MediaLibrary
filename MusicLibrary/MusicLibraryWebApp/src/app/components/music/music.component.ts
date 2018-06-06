import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AlbumService } from '../../services/album.service';
import { ArtistService } from '../../services/artist.service';
import { TrackService } from '../../services/track.service';

import { Album } from '../../shared/models/album.model';
import { Track } from '../../shared/models/track.model';
import { Artist } from '../../shared/models/artist.model';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  tracks: Track[];
  artists: Artist[];
  albums: Album[];

  constructor(private trackService: TrackService, private artistService: ArtistService,
              private albumService: AlbumService) { }

  ngOnInit() {
    this.getTracks();
    this.getArtists();
    this.getAlbums();
  }

  getTracks(): void {
    this.trackService.getTracks().subscribe(tracks => this.tracks = tracks);
  }

  getAlbums(): void {
    this.albumService.getAlbums().subscribe(albums => this.albums = albums);
  }

  getArtists(): void {
    this.artistService.getArtists().subscribe(artists => this.artists = artists);
  }
}
