import { Injectable } from '@angular/core';
import { Track } from '../shared/models/track.model';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ITrackList, ITrackGroup } from '../shared/interfaces/music.interface';
import { TrackSortEnum } from '../components/music/enums/music-enum';
import { TrackRowComponent } from '../components/music/track-list/track-row/track-row.component';
import { TrackListComponent } from '../components/music/track-list/track-list.component';
import { AlbumService } from './album.service';
import { ArtistService } from './artist.service';
import { GenreService } from './genre.service';
import { ITrack } from '../shared/interfaces/track.interface';

@Injectable({
  providedIn: 'root'
})

export class TrackService {
  static readonly TracksPerGroup: number = 100;

  private letters: Array<string> = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  private tracks: Track[] = [];

  constructor(private http: HttpClient, private albumService: AlbumService, private artistService: ArtistService,
    private genreService: GenreService) { }

  getTracks(refresh: boolean = false): Observable<Track[]> {
    let tracks: Observable<Track[]> = of();

    if (this.tracks.length === 0 || refresh) {
      tracks = this.http.get<ITrack[]>('/api/Track').pipe(map(data => data.map(track => new Track().deserialize(track))),
                                                          map(data => this.tracks = data));
    } else {
      tracks = of(this.tracks);
    }

    return tracks;
  }

  getTrack(id: number): Observable<Track> {
      const trackId: number = !!id ? id : -1;
      let track: Observable<Track>;
      const foundTrack: Track = this.tracks.find(_track => _track.id === trackId);

      if (!!foundTrack) {
        track = of(foundTrack);
      } else {
        track = this.http.get<Track>('/api/Track/' + id)
          .pipe(map(_track => new Track().deserialize(_track)));
      }

      return track;
  }

  getTrackSortLists(trackSort: TrackSortEnum): Observable<ITrackList[]> {
    let lists: Observable<ITrackList[]> = of();
    const tracks: Observable<Track[]> = this.getTracks(true);

    lists = tracks.pipe(map((_tracks, _index) => {
      let _lists: ITrackList[] = [];

      this.updateTracks(_tracks);

      switch (trackSort) {
        case TrackSortEnum.Album:
          _lists = this.getAlbumIds(_tracks).map(albumId => ({
            title: this.albumService.getAlbum(albumId).pipe(map(album => album.title)),
            groups: this.splitTracksIntoGroups(_tracks.filter((track, i, a) => track.albumId === albumId))
          }));
          break;
        case TrackSortEnum.Artist:
          _lists = this.getArtistIds(_tracks).map(artistId => ({
            title: this.artistService.getArtist(artistId).pipe(map(artist => artist.name)),
            groups: this.splitTracksIntoGroups(_tracks.filter((track, i, a) => track.artistId === artistId))
          }));
          break;
        case TrackSortEnum.AtoZ:
          _lists = ['&', '#'].concat(this.letters)
            .map(char => ({
              title: of(char),
              groups: this.splitTracksIntoGroups(this.getTracksAtoZ(char, _tracks))
            }));
          break;
        case TrackSortEnum.DateAdded:
          const dates = _tracks.map(track => track.createDate.toDateString()).sort().reverse();
          _lists = dates.filter((date, index) => dates.findIndex(item => item === date) === index).map(date => ({
              title: of(date),
              groups: this.splitTracksIntoGroups(_tracks.filter(track => track.createDate.toDateString() === date))
            }));
          break;
        case TrackSortEnum.None:
        default:
          _lists = [];
          break;
      }

      _lists.forEach(list => list.height = this.getTrackListHeight(list.groups));

      return _lists;
    }));

    return lists.pipe(map(_lists => _lists.filter(list => list.groups.length > 0)));
  }

  getTrackListHeight(groups: ITrackGroup[]): number {
    return (groups.reduce((acc, current) => acc + current.tracks.length, 0) * TrackRowComponent.TrackHeight) +
            TrackListComponent.HeaderHeight;
  }

  getTracksAtoZ(char: string, tracks: Track[]): Track[] {
    let _tracks: Track[];

    switch (char) {
      case '&':
        _tracks = tracks.filter((track, index, _) => isNaN(parseInt(track.title[0], 10)) &&
          !this.letters.includes(track.title[0].toUpperCase()));
        break;
      case '#':
        _tracks = tracks.filter(track => !isNaN(parseInt(track.title[0], 10)));
        break;
      default:
        _tracks = tracks.filter(track => track.title[0].toUpperCase() === char);
        break;
    }

    return _tracks;
  }

  getAlbumIds(tracks: Track[]): number[] {
    return tracks.map(track => track.albumId)
      .filter((albumId, index, _albumIds) => _albumIds.findIndex(item => item === albumId) === index);
  }

  getArtistIds(tracks: Track[]): number[] {
    return tracks.map(track => track.artistId)
      .filter((artistId, index, _artistIds) => _artistIds.findIndex(item => item === artistId) === index);
  }

  splitTracksIntoGroups(tracks: Track[]): ITrackGroup[] {
    const tracksPerGroup = TrackService.TracksPerGroup,
          groupCount: number = Math.ceil(tracks.length / tracksPerGroup);

    return Array.from(Array(groupCount).keys()).map(index => {
      const start: number = index * tracksPerGroup,
            end: number = start + tracksPerGroup;
      const _tracks: Track[] = tracks.slice(start, end);
      return ({
        tracks: _tracks,
        height: _tracks.length * TrackRowComponent.TrackHeight
      });
    });
  }

  updateTracks(tracks: Track[]): void {
    tracks.forEach(track => {
      track.album = this.albumService.getAlbum(track.albumId).pipe(map(album => album.title));
      track.artist = this.artistService.getArtist(track.artistId).pipe(map(artist => artist.name));
      track.genre = this.genreService.getGenre(track.genreId).pipe(map(genre => genre.name));
    });
  }
}
