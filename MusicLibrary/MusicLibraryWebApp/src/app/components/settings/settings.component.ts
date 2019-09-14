import { Component, OnInit, AfterViewInit } from '@angular/core';
import { MusicService } from 'src/app/services/music.service';
import { IPathData } from 'src/app/shared/interfaces/music.interface';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit, AfterViewInit {
  private browseModal: JQuery<HTMLElement>;
  private addModal: JQuery<HTMLElement>;
  private paths$: Observable<string[]>;

  constructor(private musicService: MusicService) { }

  ngOnInit() {
    // this.paths$ = this.musicService.getPaths();
  }

  ngAfterViewInit(): void {
    this.browseModal = $('#browseModal');
    this.addModal = $('#addPathsModal');
    this.addModal.on('show.bs.modal', evt => {
      this.addModal.find('.modal-title').text();
      this.addModal.find('.modal-body button').val();
    });
    this.browseModal.on('show.bs.modal', evt => {
      this.browseModal.find('.modal-body input[type="text"]').val('');
      this.browseModal.find('.modal-body input[type="checkbox"]').val('false');
    });
  }

  addPaths() {
    const data: IPathData = { paths: [], copy: false, recursive: false };

    this.musicService.addPaths(data).subscribe(null, response => console.error(response));
  }
}
