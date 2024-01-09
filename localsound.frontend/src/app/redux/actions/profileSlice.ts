import { createSlice } from "@reduxjs/toolkit";
import { ProfileState } from "../../model/redux/profileState";

const initialState: ProfileState = {
  profileData: null,
};
export const applicationSlice = createSlice({
  name: "profile",
  initialState,
  reducers: {
    handleSetProfile: (state = initialState, action) => {
      state.profileData = action.payload;
    },
    handleUpdateAllowAddPackage: (state = initialState, action) => {
      if (state.profileData) state.profileData.canAddPackage = action.payload;
    },
    handleUpdateProfileFollowCount: (state = initialState, action) => {
      if (state.profileData) state.profileData.followerCount = action.payload;
    },
  },
});

export const {
  handleSetProfile,
  handleUpdateAllowAddPackage,
  handleUpdateProfileFollowCount,
} = applicationSlice.actions;

export default applicationSlice.reducer;
