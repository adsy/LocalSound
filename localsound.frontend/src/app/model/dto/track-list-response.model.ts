import { ArtistTrackUploadModel } from "./artist-track-upload.model";

export interface TrackListResponse {
  trackList: ArtistTrackUploadModel[];
  canLoadMore: boolean;
}
