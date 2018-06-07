import { Component, OnInit, Input } from '@angular/core';
import { Album } from '../../../shared/models/album.model';
import { Artist } from '../../../shared/models/artist.model';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})

export class AlbumComponent implements OnInit {
  @Input() album: Album;
  @Input() artists: Artist[];

  constructor() { }

  ngOnInit() {
  }

  getArtistNameById(id: number): string {
    const foundArtist = this.artists.find(artist => artist.id === id);
    return foundArtist !== undefined && foundArtist !== null ? foundArtist.name : '';
  }
}
