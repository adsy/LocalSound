import { ArtistTrackModel } from "../dto/artist-track-upload.model";
import { PlaylistTypes } from "../enums/playlistTypes";
import { CurrentSongState } from "./currentSongState";

export interface PlayerState {
  listeningProfileMemberId: string | null;
  canLoadMore: boolean;
  page: number;
  trackList: ArtistTrackModel[];
  playlistType: PlaylistTypes | null;
  currentSong: CurrentSongState | null;
}
