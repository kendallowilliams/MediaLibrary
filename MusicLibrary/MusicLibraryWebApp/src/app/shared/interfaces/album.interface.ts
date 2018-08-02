import { Observable } from '../../../../node_modules/rxjs';

export interface IAlbum {
  id: number;
  title: string;
  artistId: number;
  artist: Observable<string>;
  genreId: number;
  genre: Observable<string>;
  year: number;
  createDate: Date;
}
