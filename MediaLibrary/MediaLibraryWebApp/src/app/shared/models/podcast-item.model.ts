import { Observable } from 'rxjs';
import { IPodcastItem } from '../interfaces/podcast-item.interface';
import { Deserializable } from '../interfaces/deserializable.interface';

export class PodcastItem implements Deserializable, IPodcastItem {
  id: number;
  title: string;
  url: string;
  description: string;
  length: number;
  publishDate: Date;
  podcastId: number;

  deserialize(input: any) {
    Object.assign(this, input);
    return this;
  }
}
