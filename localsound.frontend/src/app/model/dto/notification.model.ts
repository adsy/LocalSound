export interface NotificationModel {
  notificationId: number;
  receiverMemberId: string;
  creatorMemberId: string;
  notificationMessage: string;
  redirectUrl: string;
  notificationViewed: boolean;
  userImage: string;
  createdOn: Date;
}
