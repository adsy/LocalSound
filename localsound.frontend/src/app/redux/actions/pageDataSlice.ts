import { createSlice } from "@reduxjs/toolkit";
import { PageDataState } from "../../model/redux/pageDataState";
import { AccountImageTypes } from "../../model/enums/accountImageTypes";

const initialState: PageDataState = {
  profileData: null,
};
export const pageDataSlice = createSlice({
  name: "pageData",
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
    handleUpdateProfileCoverPhoto: (state = initialState, action) => {
      if (state.profileData) {
        var images = state.profileData.images.filter(
          (x) => x.accountImageTypeId === AccountImageTypes.ProfileImage
        );
        images.push(action.payload);
        state.profileData.images = images;
      }
    },
    handleUpdateProfilePhoto: (state = initialState, action) => {
      if (state.profileData) {
        var images = state.profileData.images.filter(
          (x) => x.accountImageTypeId === AccountImageTypes.CoverImage
        );
        images.push(action.payload);
        state.profileData.images = images;
      }
    },
  },
});

export const {
  handleSetProfile,
  handleUpdateAllowAddPackage,
  handleUpdateProfileFollowCount,
  handleUpdateProfileCoverPhoto,
  handleUpdateProfilePhoto,
} = pageDataSlice.actions;

export default pageDataSlice.reducer;
