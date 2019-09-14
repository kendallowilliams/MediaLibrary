import { Deserializable } from '../interfaces/deserializable.interface';
import { IArtist } from '../interfaces/artist.interface';

export class Artist implements Deserializable, IArtist {
    id: number;
    name: string;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
