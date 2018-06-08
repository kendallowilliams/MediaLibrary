import { Deserializable } from '../interfaces/deserializable.interface';

export class Track implements Deserializable {
    id: number;
    title: string;
    fileName: string;
    pathId: number;
    albumId: number;
    album: string;
    genreId: number;
    genre: string;
    artistId: number;
    artist: string;
    position: number;
    year: number;
    duration: number;
    durationDisplay: Date;
    playCount: number;
    createDate: string;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
