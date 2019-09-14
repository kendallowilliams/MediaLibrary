import { Observable } from 'rxjs';

export interface IPodcastItem {
  id: number;
  title: string;
  url: string;
  description: string;
  length: number;
  publishDate: Date;
  podcastId: number;
}
