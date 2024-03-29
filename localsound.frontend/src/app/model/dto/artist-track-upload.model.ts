import { GenreModel } from "./genre.model";

export interface ArtistTrackModel {
  artistTrackId: number;
  trackName: string;
  trackDescription: string;
  genres: GenreModel[];
  trackImageUrl: string;
  trackUrl: string;
  waveformUrl: string;
  duration: number;
  uploadDate: Date;
  artistProfile: string;
  artistName: string;
  artistMemberId: string;
  songLiked: boolean;
  likeCount: number;
  songLikeId?: number;
}
