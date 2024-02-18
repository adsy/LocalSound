import { createSlice } from "@reduxjs/toolkit";
import { PlayerState } from "../../model/redux/playerState";

const initialState: PlayerState = {
  currentSong: null,
  listeningProfileMemberId: null,
  canLoadMore: false,
  page: 0,
  trackList: [],
  playlistType: null,
};

export const playerSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleResetPlayerState: () => initialState,
    handleSetPlayerSong: (state, { payload }) => {
      state.currentSong = {
        trackId: payload.trackId,
        trackUrl: payload.trackUrl,
        playing: false,
        trackName: payload.trackName,
        trackImage: payload.trackImage,
        artistName: payload.artistName,
        currentSongArtistProfile: payload.currentSongArtistProfile,
        duration: payload.duration,
        uploadDate: payload.uploadDate,
        songLikeId: payload.songLikeId,
      };
      if (payload.playlistType) {
        state.playlistType = payload.playlistType;
      }
    },
    handlePauseSong: (state) => {
      if (state.currentSong) {
        state.currentSong.playing = false;
      }
    },
    handlePlaySong: (state) => {
      if (state.currentSong) {
        state.currentSong.playing = true;
      }
    },
    handleSetTrackList: (state, { payload }) => {
      state.trackList = payload.trackList;
      state.canLoadMore = payload.canLoadMore;
      state.page = payload.page;
      state.listeningProfileMemberId = payload.listeningProfileMemberId;
    },
  },
});

export const {
  handleResetPlayerState,
  handleSetPlayerSong,
  handlePauseSong,
  handlePlaySong,
  handleSetTrackList,
} = playerSlice.actions;

export default playerSlice.reducer;
