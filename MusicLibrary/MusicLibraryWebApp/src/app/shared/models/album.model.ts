import { Deserializable } from '../interfaces/deserializable.interface';

export class Album implements Deserializable {
    id: number;
    title: string;
    artistId: number;
    genreId: number;
    year: number;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
