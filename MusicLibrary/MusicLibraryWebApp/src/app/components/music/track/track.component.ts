import { Component, OnInit, Input } from '@angular/core';

import { Track } from '../../../shared/models/track.model';
import { Artist } from '../../../shared/models/artist.model';
import { Album } from '../../../shared/models/album.model';
import { Genre } from '../../../shared/models/genre.model';

@Component({
  selector: 'app-track',
  templateUrl: './track.component.html',
  styleUrls: ['./track.component.css']
})

export class TrackComponent implements OnInit {
  @Input() track: Track;
  @Input() artists: Artist[];
  @Input() albums: Album[];
  @Input() genres: Genre[];

  constructor() { }

  ngOnInit() {
  }

  getGenreNameById(id: number): string {
    const foundGenre = this.genres.find(genre => genre.id === id);
    return foundGenre !== undefined && foundGenre !== null ? foundGenre.name : '';
  }

  getAlbumTitleById(id: number): string {
    const foundAlbum = this.albums.find(album => album.id === id);
    return foundAlbum !== undefined && foundAlbum !== null ? foundAlbum.title : '';
  }

  getArtistNameById(id: number): string {
    const foundArtist = this.artists.find(artist => artist.id === id);
    return foundArtist !== undefined && foundArtist !== null ? foundArtist.name : '';
  }

  getDurationDisplay(duration: number): string {
    const seconds = duration % 60,
          minutes = (duration - seconds) / 60;
    return minutes + ':' + (seconds < 10 ? '0' + seconds : seconds);
  }
}
