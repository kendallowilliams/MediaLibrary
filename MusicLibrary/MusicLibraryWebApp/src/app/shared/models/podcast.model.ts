import { Deserializable } from '../interfaces/deserializable.interface';
import { Track } from './track.model';

export class Podcast implements Deserializable {
    id: number;
    title: string;
    url: string;
    content: string;
    lastUpdateDate: Date;
    tracks: Track[];

    deserialize(input: any) {
        Object.assign(this, input);
        this.tracks = [].concat(input.tracks).map(track => new Track().deserialize(track));
        return this;
    }
}
