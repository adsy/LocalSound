import { createSlice } from "@reduxjs/toolkit";
import { UserState } from "../../model/redux/userState";
import { AccountImageTypes } from "../../model/enums/accountImageTypes";

const initialState: UserState = {
  userDetails: null,
};

export const userSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleResetUserState: () => initialState,
    handleSetUserDetails: (state, { payload }) => {
      state.userDetails = payload;
    },
    handleUpdateUserCoverPhoto: (state, { payload }) => {
      var images = state.userDetails?.images.filter(
        (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
      );

      var newArray = [...images!, payload];

      var user = {
        ...state.userDetails!,
        images: newArray,
      };

      state.userDetails = user;
    },
    handleUpdateUserProfilePhoto: (state, { payload }) => {
      var images = state.userDetails?.images.filter(
        (x) => x.accountImageTypeId == AccountImageTypes.CoverImage
      );

      var newArray = [...images!, payload];

      var user = {
        ...state.userDetails!,
        images: newArray,
      };

      state.userDetails = user;
    },
    handleUpdateUserPackages: (state, { payload }) => {
      var user = {
        ...state.userDetails!,
        packages: payload,
      };

      state.userDetails = user;
    },
  },
});

export const {
  handleResetUserState,
  handleSetUserDetails,
  handleUpdateUserCoverPhoto,
  handleUpdateUserProfilePhoto,
  handleUpdateUserPackages,
} = userSlice.actions;

export default userSlice.reducer;
