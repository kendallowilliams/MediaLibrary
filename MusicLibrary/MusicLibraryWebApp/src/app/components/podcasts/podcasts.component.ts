import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PodcastService } from 'src/app/services/podcast.service';
import { Observable } from 'rxjs';
import { Podcast } from 'src/app/shared/models/podcast.model';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-podcasts',
  templateUrl: './podcasts.component.html',
  styleUrls: ['./podcasts.component.css']
})
export class PodcastsComponent implements OnInit, AfterViewInit {
  protected podcasts$: Observable<Podcast[]>;
  private podcasts: Podcast[];
  private modal: JQuery<HTMLElement>;

  constructor(private podcastService: PodcastService) { }

  ngOnInit() {
    this.podcasts$ = this.podcastService.getPodcasts().pipe(map(podcasts => this.podcasts = podcasts));
  }

  ngAfterViewInit() {
    this.modal = $('#podcastModal');
    this.modal.on('show.bs.modal', evt => {
      const button = $((evt as any).relatedTarget);
      const podcastId = button.data('podcast-id');
      const podcast: Podcast = this.podcasts.find(_podcast => _podcast.id === podcastId);
      this.modal.find('.modal-title').text(podcast.title + ' Options');
      this.modal.find('.modal-body button').val(podcast.id);
    });
  }

  refreshPodcast() {
    const unfollowBtn = this.modal.find('button[data-action="refresh"]');
    this.podcastService.deletePodcast(unfollowBtn.val() as number);
    this.modal.find('button[data-dismiss="modal"]').click();
  }

  unfollowPodcast() {
    const unfollowBtn = this.modal.find('button[data-action="unfollow"]');
    this.podcastService.deletePodcast(unfollowBtn.val() as number).subscribe();
    this.modal.find('button[data-dismiss="modal"]').click();
  }
}
