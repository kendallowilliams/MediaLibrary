import { Deserializable } from '../interfaces/deserializable.interface';

export class Album implements Deserializable {
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
        return this;
    }
}
