import { Deserializable } from '../interfaces/deserializable.interface';

export class Track implements Deserializable {
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
    playCount: number;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
