import { createSlice } from "@reduxjs/toolkit";
import { PlayerState } from "../../model/redux/playerState";
import { resourceUsage } from "process";

const initialState: PlayerState = {
  trackId: null,
  playing: false,
  trackUrl: null,
};

export const playerSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleResetPlayerState: () => initialState,
    handleSetPlayerSong: (state, { payload }) => {
      state.trackId = payload.trackId;
      state.trackUrl = payload.trackUrl;
      state.playing = true;
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
