import { ArtistTrackUploadModel } from "../dto/artist-track-upload.model";

export interface PlayerState {
  trackId: string | null;
  playing: boolean;
  trackUrl: string | null;
  trackName: string | null;
  trackImage: string | null;
  artistName: string | null;
  artistProfile: string | null;
  duration: number | null;
  canLoadMore: boolean;
  page: number;
  trackList: ArtistTrackUploadModel[];
}
