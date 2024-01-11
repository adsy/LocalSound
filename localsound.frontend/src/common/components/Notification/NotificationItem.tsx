import { Image } from "semantic-ui-react";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import placeholderImg from "../../../assets/placeholder.png";

interface Props {
  notification: NotificationModel;
}

const NotificationItem = ({ notification }: Props) => {
  return (
    <>
      <Image
        // size="mini"
        circular
        width={30}
        height={30}
        src={notification.userImage ? notification.userImage : placeholderImg}
        className="notification-image"
      ></Image>
      <div className="notification-message">
        {notification.notificationMessage}
      </div>
    </>
  );
};

export default NotificationItem;
