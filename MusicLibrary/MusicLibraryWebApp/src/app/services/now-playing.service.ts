import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { stringify } from 'querystring';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})

export class NowPlayingService {
  private trackSubject: BehaviorSubject<number>;
  private podcastSubject: BehaviorSubject<number>;
  private trackIds: number[];
  private podcastIds: number[];
  private currentTrackId: number;
  private currentPodcastId: number;

  constructor() {
    this.trackSubject = new BehaviorSubject<number>(-1);
    this.podcastSubject = new BehaviorSubject<number>(-1);
  }

  getNextTrackId(): number {
    let nextId = null;

    if (!!this.trackIds) {
      const index: number = this.trackIds.indexOf(this.currentTrackId);
      nextId = this.trackIds[index + 1];
    }

    return nextId;
  }

  getPreviousTrackId(): number {
    let previousId = null;

    if (!!this.trackIds) {
      const index: number = this.trackIds.indexOf(this.currentTrackId);
      previousId = this.trackIds[index - 1];
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

  getNextPodcastId(): number {
    let nextId = null;

    if (!!this.podcastIds) {
      const index: number = this.podcastIds.indexOf(this.currentPodcastId);
      nextId = this.podcastIds[index + 1];
    }

    return nextId;
  }

  getPreviousPodcastId(): number {
    let previousId = null;

    if (!!this.podcastIds) {
      const index: number = this.podcastIds.indexOf(this.currentPodcastId);
      previousId = this.podcastIds[index - 1];
    }

    return previousId;
  }

  setPodcastIds(podcastIds: number[]): void {
      this.podcastIds = podcastIds;
  }

  setCurrentPodcastId(id: number) {
    this.currentPodcastId = id;
    this.podcastSubject.next(this.currentPodcastId);
  }

  getCurrentPodcastId(): Observable<number> {
    return this.podcastSubject.asObservable();
  }
}
