export interface NotificationModel {
  notificationId: string;
  receiverMemberId: string;
  creatorMemberId: string;
  notificationMessage: string;
  redirectUrl: string;
  notificationViewed: boolean;
  userImage: string;
  createdOn: Date;
}
