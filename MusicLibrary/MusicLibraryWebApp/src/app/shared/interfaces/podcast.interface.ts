import { Observable } from 'rxjs';
import { PodcastItem } from '../models/podcast-item.model';

export interface IPodcast {
  id: number;
  title: string;
  url: string;
  imageUrl: string;
  description: string;
  author: string;
  content: string;
  lastUpdateDate: Date;
}
