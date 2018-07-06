import { Deserializable } from '../interfaces/deserializable.interface';
import { IAlbum } from '../interfaces/album.interface';

export class Album implements Deserializable, IAlbum {
    id: number;
    title: string;
    artistId: number;
    artist: string;
    genreId: number;
    genre: string;
    year: number;
    createDate: Date;

    deserialize(input: any) {
        Object.assign(this, input);
        this.createDate = new Date(input.createDate);
        return this;
    }
}
