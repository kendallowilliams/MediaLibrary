import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { TrackSortEnum, AlbumSortEnum, MusicTabEnum } from '../enums/music-enum';
import { Genre } from '../../../shared/models/genre.model';
import { GenreService } from '../../../services/genre.service';

@Component({
  selector: 'app-music-settings',
  templateUrl: './music-settings.component.html',
  styleUrls: ['./music-settings.component.css']
})
export class MusicSettingsComponent implements OnInit {
  public MusicTabs = MusicTabEnum;
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

  private selectedGenreId: number;
  private trackSort: TrackSortEnum;
  private albumSort: AlbumSortEnum;
  private genres: Genre[];

  @Input() itemCount: number;
  @Input() selectedMusicTab: MusicTabEnum;

  @Output("genreupdated") genreUpdated = new EventEmitter<number>();
  @Output("tracksortupdated") trackSortUpdated = new EventEmitter<TrackSortEnum>();
  @Output("albumsortupdated") albumSortUpdated = new EventEmitter<AlbumSortEnum>();

  constructor(private genreService: GenreService) { }

  ngOnInit() {
    this.genreService.getGenres().subscribe(genres => this.genres = genres);
    this.trackSortSelectionChanged(TrackSortEnum.AtoZ);
    this.albumSortSelectionChanged(AlbumSortEnum.AtoZ);
  }

  genreSelectionChanged(genreId: number): void {
    this.genreUpdated.emit(genreId);
    this.selectedGenreId = genreId;
  }

  trackSortSelectionChanged(trackSort: TrackSortEnum): void {
    this.trackSortUpdated.emit(trackSort);
    this.trackSort = trackSort;
  }

  albumSortSelectionChanged(albumSort: AlbumSortEnum): void {
    this.albumSortUpdated.emit(albumSort);
    this.albumSort = albumSort;
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
