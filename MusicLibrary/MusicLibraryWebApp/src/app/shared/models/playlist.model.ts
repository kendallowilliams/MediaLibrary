import { Deserializable } from '../interfaces/deserializable.interface';

export class Playlist implements Deserializable {
    id: number;
    name: string;
    trackIds: number[];

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
