import { Track } from "../models/track.model";
import { Album } from "../models/album.model";
import { Artist } from "../models/artist.model";

export interface ITrackList {
  title:string;
  tracks: Track[];
  loadCallback?();
}

export interface IAlbumList {
  title:string;
  albums: Album[];
}

export interface IArtistList {
  title:string;
  artists: Artist[];
}
