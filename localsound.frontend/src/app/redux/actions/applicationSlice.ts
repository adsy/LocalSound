import { createSlice } from "@reduxjs/toolkit";
import { AppState } from "../../model/redux/appState";

const initialState: AppState = {
  appLoading: true,
  userType: null,
};
export const applicationSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleAppLoading: (state = initialState, action) => {
      state.appLoading = action.payload;
    },
    handleResetAppState: () => initialState,
  },
});

export const { handleAppLoading, handleResetAppState } =
  applicationSlice.actions;

export default applicationSlice.reducer;
