import { ArtistTrackUploadModel } from "../dto/artist-track-upload.model";
import { PlaylistTypes } from "../enums/playlistTypes";

export interface PlayerState {
  trackId: string | null;
  playing: boolean;
  trackUrl: string | null;
  trackName: string | null;
  trackImage: string | null;
  artistName: string | null;
  listeningProfile: string | null;
  duration: number | null;
  canLoadMore: boolean;
  page: number;
  trackList: ArtistTrackUploadModel[];
  playlistType: PlaylistTypes;
}
