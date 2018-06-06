import { Deserializable } from '../interfaces/deserializable.interface';

export class Artist implements Deserializable {
    title: string;
    artistId: number;
    genreId: number;
    year: number;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
