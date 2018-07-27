import { Track } from '../models/track.model';
import { Album } from '../models/album.model';
import { Artist } from '../models/artist.model';

export interface ITrackList {
  title: string;
  tracks: Track[];
  height?: number;
  showTracks?(top: number, bottom: number);
  hideTracks?();
}

export interface IAlbumList {
  title: string;
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
