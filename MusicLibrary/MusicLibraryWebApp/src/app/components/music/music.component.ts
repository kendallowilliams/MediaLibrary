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

enum TrackSortEnum {
  DateAdded,
  AtoZ,
  Artist,
  Album
}

enum AlbumSortEnum {
  DateAdded,
  AtoZ,
  ReleaseYear,
  Artist
}

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
  currentTrackSort: TrackSortEnum;
  currentAlbumSort: AlbumSortEnum;
  trackSortOptions: any[];
  albumSortOptions: any[];

  constructor(private trackService: TrackService, private artistService: ArtistService,
    private albumService: AlbumService, private genreService: GenreService) {
    this.currentAlbumSort = AlbumSortEnum.DateAdded;
    this.currentTrackSort = TrackSortEnum.AtoZ;
    this.trackSortOptions = [
      { id: TrackSortEnum.DateAdded, name: 'Date added' },
      { id: TrackSortEnum.AtoZ, name: 'A to Z' },
      { id: TrackSortEnum.Artist, name: 'Artist' },
      { id: TrackSortEnum.Album, name: 'Album' }
    ];
    this.albumSortOptions = [
      { id: AlbumSortEnum.DateAdded, name: 'Date added' },
      { id: AlbumSortEnum.AtoZ, name: 'A to Z' },
      { id: AlbumSortEnum.ReleaseYear, name: 'Release year' },
      { id: AlbumSortEnum.Artist, name: 'Artist' }
    ];
  }

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
