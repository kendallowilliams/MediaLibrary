import { Deserializable } from '../interfaces/deserializable.interface';

export class Playlist implements Deserializable {

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
