import { Icon, Image } from "semantic-ui-react";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import placeholderImg from "../../../assets/placeholder.png";
import { Button } from "react-bootstrap";
import { useState } from "react";

interface Props {
  notification: NotificationModel;
  queuedDeletes: string[];
  setQueuedDeletes: (ids: string[]) => void;
}

const NotificationItem = ({
  notification,
  queuedDeletes,
  setQueuedDeletes,
}: Props) => {
  const deleteNotification = async (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    e.stopPropagation();
    if (
      queuedDeletes.findIndex((x) => x === notification.notificationId) === -1
    )
      setQueuedDeletes([...queuedDeletes, notification.notificationId]);
  };

  return (
    <div className={`notification-item fade-in`}>
      <Button
        className={`transparent-button cancel-notification-btn`}
        onClick={async (e) => await deleteNotification(e)}
      >
        <h4>
          <Icon
            name="cancel"
            size="small"
            className="m-0 cancel-notification-icon"
          ></Icon>
        </h4>
      </Button>
      <div className="col-2">
        <Image
          // size="mini"
          circular
          width={35}
          height={35}
          src={notification.userImage ? notification.userImage : placeholderImg}
          className="notification-image"
        ></Image>
      </div>
      <div className="notification-message">
        {notification.notificationMessage}
      </div>
    </div>
  );
};

export default NotificationItem;
