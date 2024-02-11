import { createSlice } from "@reduxjs/toolkit";
import { UserState } from "../../model/redux/userState";
import { AccountImageTypes } from "../../model/enums/accountImageTypes";
import { MessageTypes } from "../../model/enums/messageTypes";

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
    handleSaveProfileData: (state, { payload }) => {
      state.userDetails = payload;
      if (state.userDetails) {
        state.userDetails.messages.onboardingMessageClosed = true;
      }
    },
    handleCloseMessage: (state, { payload }) => {
      if (state.userDetails && state.userDetails.messages) {
        switch (payload) {
          case MessageTypes.onboardingClosedMessage: {
            state.userDetails.messages.onboardingMessageClosed = true;
            break;
          }
          default:
            break;
        }
      }
    },
  },
});

export const {
  handleResetUserState,
  handleSetUserDetails,
  handleUpdateUserCoverPhoto,
  handleUpdateUserProfilePhoto,
  handleSaveProfileData,
  handleCloseMessage,
} = userSlice.actions;

export default userSlice.reducer;
