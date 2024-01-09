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
      state.trackList = payload.trackList;
    },
    handlePauseSong: (state) => {
      state.playing = false;
    },
    handlePlaySong: (state) => {
      state.playing = true;
    },
  },
});

export const {
  handleResetPlayerState,
  handleSetPlayerSong,
  handlePauseSong,
  handlePlaySong,
} = playerSlice.actions;

export default playerSlice.reducer;
