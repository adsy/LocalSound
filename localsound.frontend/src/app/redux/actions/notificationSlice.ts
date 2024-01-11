import { createSlice } from "@reduxjs/toolkit";
import { NotificationState } from "../../model/redux/notificationState";

const initialState: NotificationState = {
  notificationList: [],
  notificationContainerVisible: false,
};
export const notificationSlice = createSlice({
  name: "notifications",
  initialState,
  reducers: {
    handleSaveNotifications: (state = initialState, action) => {
      state.notificationList = action.payload;
    },
    handleResetNotificationState: () => initialState,
    handleSaveNotification: (state = initialState, action) => {
      state.notificationList.push(action.payload);
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
