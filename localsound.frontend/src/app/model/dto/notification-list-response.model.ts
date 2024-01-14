import { NotificationModel } from "./notification.model";

export interface NotificationListResponseModel {
  notificationList: NotificationModel[];
  canLoadMore: boolean;
  unreadNotificationCount: number;
}
