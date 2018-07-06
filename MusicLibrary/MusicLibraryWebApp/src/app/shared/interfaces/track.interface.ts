export interface ITrack {
  id: number;
  title: string;
  fileName: string;
  pathId: number;
  albumId: number;
  album: string;
  genreId: number;
  genre: string;
  artistId: number;
  artist: string;
  position: number;
  year: number;
  duration: number;
  durationDisplay: string;
  playCount: number;
  createDate: Date;
}
