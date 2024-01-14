import { createSlice } from "@reduxjs/toolkit";
import { NotificationState } from "../../model/redux/notificationState";

const initialState: NotificationState = {
  notificationList: [],
  canLoadMore: false,
  notificationContainerVisible: false,
  initialLoad: false,
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
    handleUpdateInitialLoad: (state = initialState) => {
      state.initialLoad = true;
    },
    handleUpdateNotificationToViewed: (state = initialState, action) => {
      var notification = state.notificationList.find(
        (x) => x.notificationId == action.payload.notificationId
      );

      if (notification) {
        notification.notificationViewed = true;
      }
    },
  },
});

export const {
  handleSaveNotifications,
  handleResetNotificationState,
  handleSaveNotification,
  handleShowNotificationContainer,
  handleHideNotificationContainer,
  handleUpdateInitialLoad,
  handleUpdateNotificationToViewed,
} = notificationSlice.actions;

export default notificationSlice.reducer;
