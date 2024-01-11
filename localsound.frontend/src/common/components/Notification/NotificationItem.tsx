import { Icon, Image } from "semantic-ui-react";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import placeholderImg from "../../../assets/placeholder.png";
import { Button } from "react-bootstrap";

interface Props {
  notification: NotificationModel;
}

const NotificationItem = ({ notification }: Props) => {
  const deleteNotification = async () => {};

  return (
    <div
      className={`notification-item ${
        !notification.notificationViewed ? "unviewed" : ""
      }`}
    >
      <Button
        className="transparent-button cancel-notification-btn"
        onClick={async () => await deleteNotification()}
      >
        <h4>
          <Icon
            name="cancel"
            size="small"
            className="m-0 cancel-notification-icon"
          ></Icon>
        </h4>
      </Button>
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
    </div>
  );
};

export default NotificationItem;
