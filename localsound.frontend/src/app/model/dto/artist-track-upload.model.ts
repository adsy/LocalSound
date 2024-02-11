import { GenreModel } from "./genre.model";

export interface ArtistTrackUploadModel {
  artistTrackUploadId: string;
  trackName: string;
  trackDescription: string;
  genres: GenreModel[];
  trackImageUrl: string;
  trackDataLocation: string;
  trackUrl: string;
  waveformUrl: string;
  duration: number;
  uploadDate: Date;
  artistProfile: string;
  artistName: string;
  artistMemberId: string;
  songLiked: boolean;
  likeCount: number;
}
