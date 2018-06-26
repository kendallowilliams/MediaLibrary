import { Component, OnInit, Input, ElementRef, Output, APP_INITIALIZER } from '@angular/core';
import { Album } from '../../../shared/models/album.model';
import { IAlbumList } from '../../../shared/interfaces/music.interface';

@Component({
  selector: 'app-album-list',
  templateUrl: './album-list.component.html',
  styleUrls: ['./album-list.component.css']
})
export class AlbumListComponent implements OnInit {
  @Input() group: IAlbumList;
  @Input() albums: Album[];

  private height: number;
  private top: number;


  constructor() {
  }

  ngOnInit() {
    this.albums = this.group.albums;
  }

  trackByAlbums(index: number, album: Album): number {
    return album.id;
  }
}
