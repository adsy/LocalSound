import { createSlice } from "@reduxjs/toolkit";
import { UserState } from "../../model/redux/userState";

const initialState: UserState = {
  userDetails: null,
};

export const userSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleResetState: () => initialState,
    handleSetUserDetails: (state, { payload }) => {
      state.userDetails = payload;
    },
  },
});

export const { handleResetState, handleSetUserDetails } = userSlice.actions;

export default userSlice.reducer;
