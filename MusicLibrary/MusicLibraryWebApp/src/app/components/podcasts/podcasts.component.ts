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
  private editModal: JQuery<HTMLElement>;
  private addModal: JQuery<HTMLElement>;

  constructor(private podcastService: PodcastService) { }

  ngOnInit() {
    this.reloadPodcasts();
  }

  ngAfterViewInit() {
    this.editModal = $('#editPodcastModal');
    this.addModal = $('#addPodcastModal');
    this.editModal.on('show.bs.modal', evt => {
      const button = $((evt as any).relatedTarget);
      const podcastId = button.data('podcast-id');
      const podcast: Podcast = this.podcasts.find(_podcast => _podcast.id === podcastId);
      this.editModal.find('.editModal-title').text(podcast.title + ' Options');
      this.editModal.find('.modal-body button').val(podcast.id);
    });
    this.addModal.on('show.bs.modal', evt => {
      this.addModal.find('.modal-body input[type="text"]').val('');
      this.addModal.find('.modal-body input[type="checkbox"]').val('false');
    });
  }

  refreshPodcast() {
    const unfollowBtn = this.editModal.find('button[data-action="refresh"]');
    const podcast: Podcast = this.podcasts.find(_podcast => _podcast.id === parseInt(unfollowBtn.val() as string, null));
    this.podcastService.refreshPodcast(podcast).subscribe(_podcast => {
      podcast.author = _podcast.author;
      podcast.description = _podcast.description;
      podcast.imageUrl = _podcast.imageUrl;
      podcast.lastUpdateDate = _podcast.lastUpdateDate;
      podcast.title = _podcast.title;
      podcast.url = _podcast.url;
    });
    this.editModal.find('button[data-dismiss="modal"]').click();
  }

  unfollowPodcast() {
    const unfollowBtn = this.editModal.find('button[data-action="unfollow"]');
    const podcast: Podcast = this.podcasts.find(_podcast => _podcast.id === parseInt(unfollowBtn.val() as string, null));
    this.podcastService.deletePodcast(podcast.id).subscribe(success => {
      const index: number = this.podcasts.indexOf(podcast);
      if (success) { this.podcasts.splice(index, 1); }
    });
    this.editModal.find('button[data-dismiss="modal"]').click();
  }

  reloadPodcasts() {
    this.podcasts$ = this.podcastService.getPodcasts().pipe(map(podcasts => this.podcasts = podcasts));
  }

  addPodcast() {
    const feed = this.addModal.find('.modal-body input[type="text"]').val();
    const copy = this.addModal.find('.modal-body input[type="checkbox"]').prop('checked');
    this.podcastService.addPodcast(feed as string, copy).subscribe(podcasts => {
      this.podcasts.push(...podcasts);
    });
  }
}
