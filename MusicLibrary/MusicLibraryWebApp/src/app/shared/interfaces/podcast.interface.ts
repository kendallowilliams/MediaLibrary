import { Observable } from 'rxjs';

export interface IPodcast {
  id: number;
  title: string;
  url: string;
  imageUrl: string;
  description: string;
  content: string;
  lastUpdateDate: Date;
}
