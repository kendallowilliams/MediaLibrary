import { Observable } from '../../../../node_modules/rxjs';

export interface IAlbum {
  id: number;
  title: string;
  artistId: number;
  genreId: number;
  year: number;
  createDate: Date;
}
