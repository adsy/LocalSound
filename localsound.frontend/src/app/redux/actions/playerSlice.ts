import { createSlice } from "@reduxjs/toolkit";
import { PlayerState } from "../../model/redux/playerState";

const initialState: PlayerState = {
  trackId: null,
  playing: false,
  trackUrl: null,
  trackName: null,
  trackImage: null,
  artistName: null,
  artistProfile: null,
  duration: null,
  canLoadMore: false,
  page: 0,
  trackList: [],
};

export const playerSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleResetPlayerState: () => initialState,
    handleSetPlayerSong: (state, { payload }) => {
      state.trackId = payload.trackId;
      state.trackUrl = payload.trackUrl;
      state.playing = false;
      state.trackName = payload.trackName;
      state.trackImage = payload.trackImage;
      state.artistName = payload.artistName;
      state.artistProfile = payload.artistProfile;
      state.duration = payload.duration;
    },
    handlePauseSong: (state) => {
      state.playing = false;
    },
    handlePlaySong: (state) => {
      state.playing = true;
    },
    handleSetTrackList: (state, { payload }) => {
      state.trackList = payload.trackList;
      state.canLoadMore = payload.canLoadMore;
      state.page = payload.page;
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
