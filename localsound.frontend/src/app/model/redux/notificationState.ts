import { NotificationModel } from "../dto/notification.model";

export interface NotificationState {
  notificationList: NotificationModel[];
  canLoadMore: boolean;
  notificationContainerVisible: boolean;
  page: number;
}
