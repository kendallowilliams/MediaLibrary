import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-track-list',
  templateUrl: './track-list.component.html',
  styleUrls: ['./track-list.component.css']
})
export class TrackListComponent implements OnInit {
  @Input() trackSortGroups: any[];

  constructor() { }

  ngOnInit() {
  }
}
