import { createSlice } from "@reduxjs/toolkit";
import { AppState } from "../../model/redux/appState";

const initialState: AppState = {
  appLoaded: false,
  userType: null,
};
export const applicationSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleAppLoaded: (state = initialState, action) => {
      state.appLoaded = action.payload;
    },
    handleResetAppState: () => initialState,
  },
});

export const { handleAppLoaded, handleResetAppState } =
  applicationSlice.actions;

export default applicationSlice.reducer;
