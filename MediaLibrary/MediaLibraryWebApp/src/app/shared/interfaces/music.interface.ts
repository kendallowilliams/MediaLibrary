import { Track } from '../models/track.model';
import { Album } from '../models/album.model';
import { Artist } from '../models/artist.model';
import { Observable } from '../../../../node_modules/rxjs';

export interface ITrackList {
  title: Observable<string>;
  groups: ITrackGroup[];
  height?: number;
  showTracks?(top: number, bottom: number);
  hideTracks?();
}

export interface IAlbumList {
  title: Observable<string>;
  albums: Album[];
}

export interface IArtistList {
  title: string;
  artists: Artist[];
}

export interface ITrackGroup {
  visible?: boolean;
  height?: number;
  tracks: Track[];
}

export interface IScrollData {
  top: number;
  height: number;
  timeout: number;
  timeoutId?: any;
}

export interface IPathData {
  paths: string[];
  copy: boolean;
  recursive: boolean;
}
