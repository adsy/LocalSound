import { ArtistTrackUploadModel } from "../dto/artist-track-upload.model";
import { PlaylistTypes } from "../enums/playlistTypes";
import { CurrentSongState } from "./currentSongState";

export interface PlayerState {
  listeningProfileMemberId: string | null;
  canLoadMore: boolean;
  page: number;
  trackList: ArtistTrackUploadModel[];
  playlistType: PlaylistTypes | null;
  currentSong: CurrentSongState | null;
}
