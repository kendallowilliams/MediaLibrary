import { Observable } from 'rxjs';

export interface IPodcast {
  id: number;
  title: string;
  url: string;
  content: string;
  lastUpdateDate: Date;
}
