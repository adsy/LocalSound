import { Image } from "semantic-ui-react";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import placeholderImg from "../../../assets/placeholder.png";
import { formatDate } from "../../helper";

interface Props {
  notification: NotificationModel;
}

const NotificationItem = ({ notification }: Props) => {
  return (
    <div className={`notification-item fade-in`}>
      <p className="mb-0 mt-1 ml-1 notification-date">
        {formatDate(new Date(notification.createdOn))}
      </p>
      <div className="d-flex flex-row col-12 pb-3">
        <div className="col-2 d-flex align-items-center justify-content-center">
          <Image
            // size="mini"
            circular
            width={35}
            height={35}
            src={
              notification.userImage ? notification.userImage : placeholderImg
            }
            className="notification-image"
          ></Image>
        </div>
        <div className="col-10 d-flex align-items-center">
          <div className="notification-message">
            {notification.notificationMessage}
          </div>
        </div>
      </div>
    </div>
  );
};

export default NotificationItem;
