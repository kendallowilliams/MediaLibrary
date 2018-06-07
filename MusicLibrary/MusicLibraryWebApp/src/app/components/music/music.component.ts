import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AlbumService } from '../../services/album.service';
import { ArtistService } from '../../services/artist.service';
import { TrackService } from '../../services/track.service';
import { GenreService } from '../../services/genre.service';

import { Album } from '../../shared/models/album.model';
import { Track } from '../../shared/models/track.model';
import { Artist } from '../../shared/models/artist.model';
import { Genre } from '../../shared/models/genre.model';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  tracks: Track[];
  artists: Artist[];
  albums: Album[];
  genres: Genre[];

  constructor(private trackService: TrackService, private artistService: ArtistService,
              private albumService: AlbumService, private genreService: GenreService) { }

  ngOnInit() {
    this.getTracks();
    this.getArtists();
    this.getAlbums();
    this.getGenres();
  }

  getGenres(): void {
    this.genreService.getGenres().subscribe(genres => this.genres = genres);
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
