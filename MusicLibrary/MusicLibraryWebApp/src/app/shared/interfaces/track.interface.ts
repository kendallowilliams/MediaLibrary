import { Observable } from '../../../../node_modules/rxjs';

export interface ITrack {
  id: number;
  title: string;
  fileName: string;
  pathId: number;
  albumId: number;
  album: Observable<string>;
  genreId: number;
  genre: Observable<string>;
  artistId: number;
  artist: Observable<string>;
  position: number;
  year: number;
  duration: number;
  durationDisplay: string;
  playCount: number;
  createDate: Date;
}
