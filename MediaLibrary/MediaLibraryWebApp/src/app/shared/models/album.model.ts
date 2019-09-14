import { Deserializable } from '../interfaces/deserializable.interface';
import { IAlbum } from '../interfaces/album.interface';
import { Observable } from '../../../../node_modules/rxjs';

export class Album implements Deserializable, IAlbum {
    id: number;
    title: string;
    artistId: number;
    artist: Observable<string>;
    genreId: number;
    genre: Observable<string>;
    year: number;
    createDate: Date;

    deserialize(input: any) {
        Object.assign(this, input);
        this.createDate = new Date(input.createDate);
        return this;
    }
}
