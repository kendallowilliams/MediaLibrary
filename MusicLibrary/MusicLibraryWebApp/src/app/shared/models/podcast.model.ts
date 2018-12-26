import { Deserializable } from '../interfaces/deserializable.interface';
import { Track } from './track.model';
import { IPodcast } from '../interfaces/podcast.interface';
import { PodcastItem } from './podcast-item.model';
import { Observable } from 'rxjs';

export class Podcast implements Deserializable, IPodcast {
    id: number;
    title: string;
    url: string;
    imageUrl: string;
    description: string;
    author: string;
    content: string;
    lastUpdateDate: Date;

    deserialize(input: any) {
        Object.assign(this, input);
        this.lastUpdateDate = new Date(input.lastUpdateDate);
        return this;
    }
}
