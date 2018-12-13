import { Deserializable } from '../interfaces/deserializable.interface';
import { Track } from './track.model';

export class Podcast implements Deserializable {
    id: number;
    title: string;
    url: string;
    content: string;
    lastUpdateDate: Date;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
