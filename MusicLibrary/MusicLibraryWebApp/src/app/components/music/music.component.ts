import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AlbumService } from '../../services/album.service';
import { ArtistService } from '../../services/artist.service';
import { TrackService } from '../../services/track.service';
import { GenreService } from '../../services/genre.service';

import { Album } from '../../shared/models/album.model';
import { Track } from '../../shared/models/track.model';
import { Artist } from '../../shared/models/artist.model';
import { Genre } from '../../shared/models/genre.model';
import { ActivatedRoute } from '@angular/router';

import { TrackSortEnum, AlbumSortEnum, MusicTabEnum } from './enums/music-enum';
import { ITrackList } from '../../shared/interfaces/music.interface';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})

export class MusicComponent implements OnInit {
  public MusicTabs = MusicTabEnum;

  letters = 'abcdefghijklmnopqrstuvwxyz'.split('').map(letter => letter.toUpperCase());
  tracks: Track[];
  artists: Artist[];
  albums: Album[];
  genres: Genre[];
  currentTrackSort: TrackSortEnum;
  currentAlbumSort: AlbumSortEnum;
  trackSortOptions: any[];
  albumSortOptions: any[];
  trackSortGroups: ITrackList[];
  selectMusicTab: MusicTabEnum;

  constructor(private trackService: TrackService, private artistService: ArtistService,
    private albumService: AlbumService, private genreService: GenreService,
    private route: ActivatedRoute) {
    this.currentAlbumSort = AlbumSortEnum.AtoZ;
    this.currentTrackSort = TrackSortEnum.AtoZ;
    this.selectMusicTab = MusicTabEnum.Songs;
  }

  ngOnInit() {
    this.getTracks();
    this.getArtists();
    this.getAlbums();
    this.getGenres();
    this.updateTracks();
    this.trackSortGroups = this.getTrackSortGroups();
  }

  getTrackSortGroups(): ITrackList[] {
    let groups = [];

    switch (this.currentTrackSort) {
      case TrackSortEnum.Album:
        groups = this.albums.map(album => album.title)
                            .map(album => ({
                              title: album,
                              tracks: this.tracks.filter(track => track.album === album)
                            }));
        break;
      case TrackSortEnum.Artist:
        groups = this.artists.map(artist => artist.name)
                             .map(artist => ({
                                title: artist,
                                tracks: this.tracks.filter(track => track.artist === artist)
                              }));
        break;
      case TrackSortEnum.AtoZ:
        groups = ['&', '#'].concat(this.letters)
                           .map(char => ({
                              title: char,
                              tracks: this.getTracksAtoZ(char)
                            }));
        break;
      case TrackSortEnum.DateAdded:
        groups = this.tracks.map(track => track.createDate)
                            .map(date => ({
                              title: date,
                              tracks: this.tracks.filter(track => track.createDate.toString() === date)
                            }));
        break;
      case TrackSortEnum.None:
      default:
        groups = [];
        break;
    }

    return groups.filter(group => group.tracks && group.tracks.length > 0);
  }

  getTracksAtoZ(char: string): Track[] {
    let tracks = null;

    switch(char)
    {
      case '&':
        tracks = this.tracks.filter(track => isNaN(parseInt(track.title[0])) &&
          !this.letters.includes(track.title[0].toUpperCase()));
        break;
      case '#':
        tracks = this.tracks.filter(track => !isNaN(parseInt(track.title[0])));
        break;
      default:
        tracks = this.tracks.filter(track => track.title[0].toUpperCase() === char);
        break;
    }

    return tracks;
  };

  updateTracks(): void {
    this.tracks.forEach(track => {
      track.album = this.getAlbumTitleById(track.albumId);
      track.artist = this.getArtistNameById(track.artistId);
      track.genre = this.getGenreNameById(track.genreId);
    });
  }

  getGenres(): void {
    this.genres = this.route.snapshot.data['genres'];
  }

  getTracks(): void {
    this.tracks = this.route.snapshot.data['tracks'];
  }

  getAlbums(): void {
    this.albums = this.route.snapshot.data['albums'];
  }

  getArtists(): void {
    this.artists = this.route.snapshot.data['artists'];
  }

  getGenreNameById(id: number): string {
    const foundGenre = this.genres.find(genre => genre.id === id);
    return foundGenre !== undefined && foundGenre !== null ? foundGenre.name : '';
  }

  getAlbumTitleById(id: number): string {
    const foundAlbum = this.albums.find(album => album.id === id);
    return foundAlbum !== undefined && foundAlbum !== null ? foundAlbum.title : '';
  }

  getArtistNameById(id: number): string {
    const foundArtist = this.artists.find(artist => artist.id === id);
    return foundArtist !== undefined && foundArtist !== null ? foundArtist.name : '';
  }
}
