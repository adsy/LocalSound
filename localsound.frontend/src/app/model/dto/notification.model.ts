export interface NotificationModel {
  notificationId: string;
  receiverMemberId: string;
  creatorMemberId: string;
  notificationMessage: string;
  redirectUrl: string;
  notificationView: boolean;
  userImage: string;
}
