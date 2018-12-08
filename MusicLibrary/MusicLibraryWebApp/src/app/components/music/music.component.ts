import { Component, OnInit, Input, Output, EventEmitter, ViewChildren, QueryList } from '@angular/core';

import { AlbumService } from '../../services/album.service';
import { ArtistService } from '../../services/artist.service';
import { TrackService } from '../../services/track.service';
import { GenreService } from '../../services/genre.service';

import { Genre } from '../../shared/models/genre.model';
import { ActivatedRoute } from '@angular/router';

import { TrackSortEnum, AlbumSortEnum, MusicTabEnum } from './enums/music-enum';
import { ITrackList, IAlbumList, IArtistList, IScrollData, ITrackGroup } from '../../shared/interfaces/music.interface';
import { AppService } from '../../services/app.service';
import { Observable } from '../../../../node_modules/rxjs';
import { TrackListComponent } from './track-list/track-list.component';
import { tap } from 'rxjs/operators';
import { IArtist } from 'src/app/shared/interfaces/artist.interface';

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
  trackCount = 0;
  artistCount = 0;
  albumCount = 0;
  artists: IArtist[] = [];
  genres: Genre[] = [];
  currentTrackSort: TrackSortEnum;
  currentAlbumSort: AlbumSortEnum;
  trackSortOptions: any[] = [];
  albumSortOptions: any[] = [];
  trackSortLists$: Observable<ITrackList[]>;
  artistSortGroups$: Observable<IArtistList[]>;
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
    const groupAcc = (acc: number, group: IArtistList) => acc + group.artists.length;
    this.artistSortGroups$ = this.artistService.getArtistSortGroups()
                                               .pipe(tap(groups => this.artistCount = groups.reduce(groupAcc, 0)));
  }

  updateMusicTab(musicTab: MusicTabEnum): void {
    this.selectMusicTab = musicTab;

    switch (musicTab) {
      case MusicTabEnum.Songs:
        this.musicCount = this.trackCount;
        break;
      case MusicTabEnum.Artists:
        this.musicCount = this.artistCount;
        break;
      case MusicTabEnum.Albums:
        this.musicCount = this.albumCount;
        break;
      case MusicTabEnum.None:
      default:
        break;
    }
  }

  updateTrackSort(trackSort: TrackSortEnum): void {
    if (this.currentTrackSort !== trackSort) {
      const groupAcc = (acc: number, group: ITrackGroup) => acc + group.tracks.length,
            listAcc = (acc: number, list: ITrackList) => acc + list.groups.reduce(groupAcc, 0);
      this.trackCount = 0;
      this.currentTrackSort = trackSort;
      this.trackSortLists$ = this.trackService.getTrackSortLists(trackSort)
                                              .pipe(tap(lists => this.trackCount = lists.reduce(listAcc, 0)),
                                                    tap(() => this.updateMusicTab(this.selectMusicTab || MusicTabEnum.Songs)));
    }
  }

  updateAlbumSort(albumSort: AlbumSortEnum): void {
    if (this.currentAlbumSort !== albumSort) {
      const listAcc = (acc: number, list: IAlbumList) => acc + list.albums.length;
      this.currentAlbumSort = albumSort;
      this.albumSortLists$ = this.albumService.getAlbumSortLists(albumSort)
                                              .pipe(tap(lists => this.albumCount = lists.reduce(listAcc, 0)));
    }
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
