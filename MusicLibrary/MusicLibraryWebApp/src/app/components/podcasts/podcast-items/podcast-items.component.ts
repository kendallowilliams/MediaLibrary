import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { PodcastItem } from 'src/app/shared/models/podcast-item.model';
import { PodcastService } from 'src/app/services/podcast.service';

@Component({
  selector: 'app-podcast-items',
  templateUrl: './podcast-items.component.html',
  styleUrls: ['./podcast-items.component.css']
})
export class PodcastItemsComponent implements OnInit, AfterViewInit {
  private podcastId: number;
  protected items$: Observable<PodcastItem[]>;

  constructor(private route: ActivatedRoute, private podcastService: PodcastService) { }

  ngOnInit() {
    this.route.params.subscribe(params => this.podcastId = params.podcastId);
  }

  ngAfterViewInit(): void {
    this.items$ = this.podcastService.getPodcastItems(this.podcastId);
  }
}
