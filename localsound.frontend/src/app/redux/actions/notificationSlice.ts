import { createSlice } from "@reduxjs/toolkit";
import { NotificationState } from "../../model/redux/notificationState";

const initialState: NotificationState = {
  notificationList: [],
  canLoadMore: false,
  notificationContainerVisible: false,
  page: 0,
};
export const notificationSlice = createSlice({
  name: "notifications",
  initialState,
  reducers: {
    handleSaveNotifications: (state = initialState, action) => {
      var clone = [
        ...state.notificationList,
        ...action.payload.notificationList,
      ];
      state.notificationList = clone;
      state.canLoadMore = action.payload.canLoadMore;
      if (action.payload.canLoadMore) {
        state.page += 1;
      }
    },
    handleResetNotificationState: () => initialState,
    handleSaveNotification: (state = initialState, action) => {
      var clone = [action.payload, ...state.notificationList];
      state.notificationList = clone;
    },
    handleShowNotificationContainer: (state = initialState) => {
      state.notificationContainerVisible = true;
    },
    handleHideNotificationContainer: (state = initialState) => {
      state.notificationContainerVisible = false;
    },
    handleRemoveNotification: (state = initialState, action) => {
      var clone = [...state.notificationList];
      var otherNotifications = clone.filter(
        (x) => x.notificationId !== action.payload
      );
      state.notificationList = otherNotifications;
    },
  },
});

export const {
  handleSaveNotifications,
  handleResetNotificationState,
  handleSaveNotification,
  handleShowNotificationContainer,
  handleHideNotificationContainer,
  handleRemoveNotification,
} = notificationSlice.actions;

export default notificationSlice.reducer;
