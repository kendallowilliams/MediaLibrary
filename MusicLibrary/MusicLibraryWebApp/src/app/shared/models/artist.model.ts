import { Deserializable } from '../interfaces/deserializable.interface';

export class Artist implements Deserializable {
    id: number;
    name: string;

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
