import { createSlice } from "@reduxjs/toolkit";
import { AppState } from "../../model/redux/appState";

const initialState: AppState = {
  appLoaded: false,
  userType: null,
  authenticationPage: {
    customerType: null,
    addressSelected: false,
  },
};
export const applicationSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleAppLoaded: (state = initialState, action) => {
      state.appLoaded = action.payload;
    },
    handleResetState: () => initialState,
  },
});

export const { handleAppLoaded, handleResetState } = applicationSlice.actions;

export default applicationSlice.reducer;
