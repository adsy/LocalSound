import { createSlice } from "@reduxjs/toolkit";
import { PageOperationState } from "../../model/redux/pageOperationState";
import { UploadTrackState } from "../../model/redux/pageOperationState.UploadTracks";

const initialState: PageOperationState = {
  uploadTracks: {
    trackUpdated: false,
    trackUploaded: false,
    trackDeleted: false,
  } as UploadTrackState,
};

export const actionSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleResetUploadTrackState: (state = initialState) => {
      if (state.uploadTracks.trackUploaded) {
        state.uploadTracks.trackUploaded = false;
      }
      if (state.uploadTracks.trackUpdated) {
        state.uploadTracks.trackUpdated = false;
      }
      if (state.uploadTracks.trackDeleted) {
        state.uploadTracks.trackDeleted = false;
      }
    },
    handleTrackUpdated: (state = initialState, action) => {
      state.uploadTracks.trackUpdated = action.payload;
      if (state.uploadTracks.trackUploaded) {
        state.uploadTracks.trackUploaded = false;
      }
      if (state.uploadTracks.trackDeleted) {
        state.uploadTracks.trackDeleted = false;
      }
    },
    handleTrackUploaded: (state = initialState, action) => {
      state.uploadTracks.trackUploaded = action.payload;
      if (state.uploadTracks.trackUpdated) {
        state.uploadTracks.trackUpdated = false;
      }
      if (state.uploadTracks.trackDeleted) {
        state.uploadTracks.trackDeleted = false;
      }
    },
    handleTrackDeleted: (state = initialState, action) => {
      state.uploadTracks.trackDeleted = action.payload;
      if (state.uploadTracks.trackUpdated) {
        state.uploadTracks.trackUpdated = false;
      }
      if (state.uploadTracks.trackUploaded) {
        state.uploadTracks.trackUploaded = false;
      }
    },
  },
});

export const {
  handleTrackUpdated,
  handleTrackUploaded,
  handleTrackDeleted,
  handleResetUploadTrackState,
} = actionSlice.actions;

export default actionSlice.reducer;
