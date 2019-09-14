import { Observable } from '../../../../node_modules/rxjs';

export interface ITrack {
  id: number;
  title: string;
  fileName: string;
  pathId: number;
  albumId: number;
  genreId: number;
  artistId: number;
  position: number;
  year: number;
  duration: number;
  durationDisplay: string;
  playCount: number;
  createDate: Date;
}
