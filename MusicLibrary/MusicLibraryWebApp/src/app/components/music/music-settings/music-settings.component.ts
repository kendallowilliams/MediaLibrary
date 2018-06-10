import { Component, OnInit, Input } from '@angular/core';

import { TrackSortEnum, AlbumSortEnum, MusicTabEnum } from '../enums/music-enum';
import { Genre } from '../../../shared/models/genre.model';

@Component({
  selector: 'app-music-settings',
  templateUrl: './music-settings.component.html',
  styleUrls: ['./music-settings.component.css']
})
export class MusicSettingsComponent implements OnInit {
  trackSortOptions = [
    { id: TrackSortEnum.Album, name: this.getTrackSortEnumDisplay(TrackSortEnum.Album) },
    { id: TrackSortEnum.Artist, name: this.getTrackSortEnumDisplay(TrackSortEnum.Artist) },
    { id: TrackSortEnum.AtoZ, name: this.getTrackSortEnumDisplay(TrackSortEnum.AtoZ) },
    { id: TrackSortEnum.DateAdded, name: this.getTrackSortEnumDisplay(TrackSortEnum.DateAdded) }
  ];
  albumSortOptions = [
    { id: AlbumSortEnum.Artist, name: this.getAlbumSortEnumDisplay(AlbumSortEnum.Artist) },
    { id: AlbumSortEnum.AtoZ, name: this.getAlbumSortEnumDisplay(AlbumSortEnum.AtoZ) },
    { id: AlbumSortEnum.DateAdded, name: this.getAlbumSortEnumDisplay(AlbumSortEnum.DateAdded) },
    { id: AlbumSortEnum.ReleaseYear, name: this.getAlbumSortEnumDisplay(AlbumSortEnum.ReleaseYear) }
  ];

  @Input() itemCount: number;
  @Input() trackSort: TrackSortEnum;
  @Input() albumSort: AlbumSortEnum;
  @Input() selectedMusicTab: MusicTabEnum;
  @Input() genres: Genre[];

  constructor() { }

  ngOnInit() {
  }

  getSortDisplay(): string {
    let sortDisplay = null;

    switch (this.selectedMusicTab) {
      case MusicTabEnum.Albums:
        sortDisplay = this.getAlbumSortEnumDisplay(this.albumSort);
        break;
      case MusicTabEnum.Songs:
        sortDisplay = this.getTrackSortEnumDisplay(this.trackSort);
        break;
      case MusicTabEnum.Artists:
      case MusicTabEnum.None:
      default:
        sortDisplay = '';
        break;
    }
    return sortDisplay;
  }

  getTrackSortEnumDisplay(trackSortEnum: TrackSortEnum): string {
    let sortDisplay = null;

    switch (trackSortEnum) {
      case TrackSortEnum.Album:
        sortDisplay = 'Album';
        break;
      case TrackSortEnum.Artist:
        sortDisplay = 'Artist';
        break;
      case TrackSortEnum.AtoZ:
        sortDisplay = 'A to Z';
        break;
      case TrackSortEnum.DateAdded:
        sortDisplay = 'Date added';
        break;
      case TrackSortEnum.None:
      default:
        sortDisplay = '';
        break;
    }
    return sortDisplay;
  }

  getAlbumSortEnumDisplay(albumSortEnum: AlbumSortEnum): string {
    let sortDisplay = null;

    switch (albumSortEnum) {
      case AlbumSortEnum.Artist:
        sortDisplay = 'Artist';
        break;
      case AlbumSortEnum.AtoZ:
        sortDisplay = 'A to Z';
        break;
      case AlbumSortEnum.DateAdded:
        sortDisplay = 'Date added';
        break;
      case AlbumSortEnum.ReleaseYear:
        sortDisplay = 'Release year';
        break;
      case AlbumSortEnum.None:
      default:
        sortDisplay = '';
        break;
    }
    return sortDisplay;
  }
}
