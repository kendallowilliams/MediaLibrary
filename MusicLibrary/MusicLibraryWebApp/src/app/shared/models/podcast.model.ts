import { Deserializable } from '../interfaces/deserializable.interface';

export class Podcast implements Deserializable {

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
