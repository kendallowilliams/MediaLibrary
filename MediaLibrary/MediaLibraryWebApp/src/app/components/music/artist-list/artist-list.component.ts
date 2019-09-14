import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER } from '@angular/core';
import { Artist } from '../../../shared/models/artist.model';
import { IArtistList } from '../../../shared/interfaces/music.interface';

@Component({
  selector: 'app-artist-list',
  templateUrl: './artist-list.component.html',
  styleUrls: ['./artist-list.component.css']
})
export class ArtistListComponent implements OnInit {
  @Input() group: IArtistList;
  @Input() artists: Artist[];

  private height: number;
  private top: number;


  constructor() {
  }

  ngOnInit() {
    this.artists = this.group.artists;
  }

  artistByArtists(index: number, artist: Artist): number {
    return artist.id;
  }
}
