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
    durationDisplay: string;
    playCount: number;
    createDate: Date;

    deserialize(input: any) {
        Object.assign(this, input);
        this.createDate = new Date(input.createDate);
        return this;
    }
}
