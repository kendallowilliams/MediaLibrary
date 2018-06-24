import { Track } from "../models/track.model";
import { Album } from "../models/album.model";
import { Artist } from "../models/artist.model";

export interface ITrackList {
  title:string;
  tracks: Track[];
  load();
}

export interface IAlbumList {
  title:string;
  tracks: Album[];
}

export interface IArtistList {
  title:string;
  tracks: Artist[];
}
