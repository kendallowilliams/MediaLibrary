import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { stringify } from 'querystring';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})

export class NowPlayingService {
  private trackSubject: BehaviorSubject<number>;
  private trackIds: number[];
  private currentTrackId: number;

  constructor() {
    this.trackSubject = new BehaviorSubject<number>(-1);
  }

  setNextTrackId(): number {
    let nextId = null;

    if (!!this.trackIds) {
      const index: number = this.trackIds.indexOf(this.currentTrackId);
      nextId = this.trackIds[index + 1];
      if (!!nextId) {
        this.trackSubject.next(nextId);
        this.currentTrackId = nextId;
      }
    }

    return nextId;
  }

  setPreviousTrackId(): number {
    let previousId = null;

    if (!!this.trackIds) {
      const index: number = this.trackIds.indexOf(this.currentTrackId);
      previousId = this.trackIds[index - 1];
      if (!!previousId) {
        this.trackSubject.next(previousId);
        this.currentTrackId = previousId;
      }
    }

    return previousId;
  }

  setTrackIds(trackIds: number[]): void {
      this.trackIds = trackIds;
  }

  setCurrentTrackId(id: number) {
    this.currentTrackId = id;
    this.trackSubject.next(this.currentTrackId);
  }

  getCurrentTrackId(): Observable<number> {
    return this.trackSubject.asObservable();
  }
}
