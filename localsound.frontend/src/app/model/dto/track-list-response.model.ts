import { ArtistTrackModel } from "./artist-track-upload.model";

export interface TrackListResponse {
  trackList: ArtistTrackModel[];
  canLoadMore: boolean;
}
