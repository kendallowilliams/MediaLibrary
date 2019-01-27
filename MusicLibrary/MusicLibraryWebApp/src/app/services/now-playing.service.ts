import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { IListItem } from 'src/app/shared/interfaces/list-item.interface';

@Injectable({
  providedIn: 'root'
})

export class NowPlayingService {
  private trackSubject: BehaviorSubject<IListItem>;
  private podcastSubject: BehaviorSubject<IListItem>;
  private trackIds: Array<IListItem>;
  private podcastIds: Array<IListItem>;
  private currentTrackId: IListItem;
  private currentPodcastId: IListItem;

  constructor() {
    this.trackSubject = new BehaviorSubject<IListItem>(null);
    this.podcastSubject = new BehaviorSubject<IListItem>(null);
  }

  getNextTrackId(repeat: boolean): IListItem  {
    let nextId: IListItem = null;

    if (!!this.trackIds) {
      const index: number = !!this.currentTrackId ? this.currentTrackId.id + 1 : 0;
      nextId = this.trackIds[index];
    }

    return nextId;
  }

  getPreviousTrackId(): IListItem {
    let previousId: IListItem = null;

    if (!!this.trackIds) {
      const index: number = !!this.currentTrackId ? this.currentTrackId.id - 1 : 0;
      previousId = this.trackIds[index];
    }

    return previousId;
  }

  setTrackIds(trackIds: number[]): void {
      this.trackIds = trackIds.map<IListItem>((_id, _index) => ({ id: _index, value: _id }));
  }

  setCurrentTrackId(item: IListItem) {
    let predicate = _item => _item.value === item.value;

    if (!!item) {
      if (!!item.id) {
        predicate = _item => _item.id === _item.id && _item.value === item.value;
      }
      this.currentTrackId = this.trackIds.find(predicate);
      this.trackSubject.next(this.currentTrackId);
    }
  }

  getCurrentTrackId(): Observable<IListItem> {
    return this.trackSubject.asObservable();
  }

  getNextPodcastId(): IListItem {
    let nextId: IListItem = null;

    if (!!this.podcastIds) {
      const index: number = !!this.currentPodcastId ? this.currentPodcastId.id + 1 : null;
      nextId = this.podcastIds[index];
    }

    return nextId;
  }

  getPreviousPodcastId(): IListItem {
    let previousId: IListItem = null;

    if (!!this.podcastIds) {
      const index: number = !!this.currentPodcastId ? this.currentPodcastId.id - 1 : null;
      previousId = this.podcastIds[index];
    }

    return previousId;
  }

  setPodcastIds(podcastIds: number[]): void {
      this.podcastIds = podcastIds.map<IListItem>((_id, _index) => ({ id: _index, value: _id }));
  }

  setCurrentPodcastId(id: number) {
    this.currentPodcastId = this.podcastIds.find(item => item.value === id);
    this.podcastSubject.next(this.currentPodcastId);
  }

  getCurrentPodcastId(): Observable<IListItem> {
    return this.podcastSubject.asObservable();
  }
}
