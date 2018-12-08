import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList } from '@angular/core';

import { AlbumService } from '../../services/album.service';
import { ArtistService } from '../../services/artist.service';
import { TrackService } from '../../services/track.service';
import { GenreService } from '../../services/genre.service';

import { Album } from '../../shared/models/album.model';
import { Track } from '../../shared/models/track.model';
import { Artist } from '../../shared/models/artist.model';
import { Genre } from '../../shared/models/genre.model';
import { ActivatedRoute } from '@angular/router';

import { TrackSortEnum, AlbumSortEnum, MusicTabEnum } from './enums/music-enum';
import { ITrackList, IAlbumList, IArtistList, IScrollData } from '../../shared/interfaces/music.interface';
import { AppService } from '../../services/app.service';
import { Observable } from '../../../../node_modules/rxjs';
import { TrackListComponent } from './track-list/track-list.component';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  public MusicTabs = MusicTabEnum;

  @Input() musicCount: number;
  @Output() play: EventEmitter<number> = new EventEmitter<number>();
  @ViewChildren(TrackListComponent) children = new QueryList<TrackListComponent>();

  letters = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  tracks: Track[] = [];
  artists: Artist[] = [];
  albums: Album[] = [];
  genres: Genre[] = [];
  currentTrackSort: TrackSortEnum;
  currentAlbumSort: AlbumSortEnum;
  trackSortOptions: any[] = [];
  albumSortOptions: any[] = [];
  trackSortLists$: Observable<ITrackList[]>;
  artistSortGroups: IArtistList[] = [];
  albumSortLists$: Observable<IAlbumList[]>;
  selectMusicTab: MusicTabEnum;
  scrollData: IScrollData = { top: 0, height: 0, timeout: 200 };

  constructor(private trackService: TrackService, private artistService: ArtistService,
    private albumService: AlbumService, private genreService: GenreService,
    private route: ActivatedRoute, private appService: AppService) {
    this.musicCount = 0;
    this.appService.musicComponent = this;
  }

  ngOnInit() {
    this.getArtists();
    this.artistSortGroups = this.getArtistSortGroups();
  }

  updateMusicTab(musicTab: MusicTabEnum): void {
    if (this.selectMusicTab !== musicTab) {
      this.selectMusicTab = musicTab;

      switch (musicTab) {
        case MusicTabEnum.Songs:
          this.musicCount = this.tracks.length;
          break;
        case MusicTabEnum.Artists:
          this.musicCount = this.artists.length;
          break;
        case MusicTabEnum.Albums:
          this.musicCount = this.albums.length;
          break;
        case MusicTabEnum.None:
        default:
          break;
      }
    }
  }

  updateTrackSort(trackSort: TrackSortEnum): void {
    if (this.currentTrackSort !== trackSort) {
      this.tracks = [];
      this.currentTrackSort = trackSort;
      this.trackSortLists$ = this.trackService.getTrackSortLists(trackSort)
                                 .pipe(tap(lists =>
                                    lists.forEach(list =>
                                      list.groups.forEach(group => Array.prototype.push.apply(this.tracks, group.tracks)))),
                                    tap(() => this.updateMusicTab(this.selectMusicTab || MusicTabEnum.Songs)));
    }
  }

  updateAlbumSort(albumSort: AlbumSortEnum): void {
    if (this.currentAlbumSort !== albumSort) {
      this.currentAlbumSort = albumSort;
      this.albumSortLists$ = this.albumService.getAlbumSortLists(albumSort)
                                              .pipe(tap(lists => this.albums = [].concat(lists.map(list => list.albums))));
    }
  }

  getArtistSortGroups(): IArtistList[] {
    return ['&', '#'].concat(this.letters).map(char => ({
      title: char,
      artists: this.getArtistsAtoZ(char)
    }));
  }

  getArtistsAtoZ(char: string): Artist[] {
    let artists = [];

    switch (char) {
      case '&':
        artists = this.artists.filter(artist => isNaN(parseInt(artist.name[0], 10)) &&
          !this.letters.includes(artist.name[0].toUpperCase()));
        break;
      case '#':
        artists = this.artists.filter(artist => !isNaN(parseInt(artist.name[0], 10)));
        break;
      default:
        artists = this.artists.filter(artist => artist.name[0].toUpperCase() === char);
        break;
    }

    return artists;
  }

  getArtists(): void {
    this.artists = this.route.snapshot.data['artists'];
  }

  handleScroll(height: number, scrollTop: number): void {
    this.scrollData.top = scrollTop;
    this.scrollData.height = height;

    if (this.scrollData.timeoutId) {
      clearTimeout(this.scrollData.timeoutId);
    }

    this.scrollData.timeoutId = setTimeout((_height, _scrollTop) => this.handleScrollCallback(_height, _scrollTop),
      this.scrollData.timeout, height, scrollTop);
  }

  handleScrollCallback(height: number, scrollTop: number): void {
    const parentTop = scrollTop,
          parentBottom = parentTop + height;

    this.children.reduce((acc, current) => {
      const listTop = acc,
            listBottom = acc + current.list.height,
            viewTop = parentTop - listTop > 0 ? parentTop - listTop : 0,
            viewBottom = viewTop + height;

      if ((listTop >= parentTop && listTop <= parentBottom) ||
          (listBottom >= parentTop && listBottom <= parentBottom) ||
          (parentTop >= listTop && parentTop <= listBottom) ||
          (parentBottom >= listTop && parentBottom <= listBottom)) {
        current.list.showTracks(viewTop, viewBottom);
      } else {
        current.list.hideTracks();
      }

      return listBottom;
    }, 0);
  }
}
