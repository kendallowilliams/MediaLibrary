import { Deserializable } from '../interfaces/deserializable.interface';

export class Album implements Deserializable {
    name: string;
    trackIds: number[];

    deserialize(input: any) {
        Object.assign(this, input);
        return this;
    }
}
