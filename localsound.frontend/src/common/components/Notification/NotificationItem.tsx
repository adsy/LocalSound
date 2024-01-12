import { Icon, Image } from "semantic-ui-react";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import placeholderImg from "../../../assets/placeholder.png";
import { Button } from "react-bootstrap";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { handleUpdateDeletingIds } from "../../../app/redux/actions/notificationSlice";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";

interface Props {
  notification: NotificationModel;
}

const NotificationItem = ({ notification }: Props) => {
  const [deleting, setDeleting] = useState(false);
  const deletingIds = useSelector(
    (state: State) => state.notifications.deletingIds
  );
  const dispatch = useDispatch();

  useEffect(() => {
    if (
      deletingIds.findIndex((x) => x === notification.notificationId) === -1
    ) {
      setDeleting(false);
    } else {
      setDeleting(true);
    }
  }, [deletingIds]);

  const deleteNotification = async (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    e.stopPropagation();
    dispatch(
      handleUpdateDeletingIds([...deletingIds, notification.notificationId])
    );
  };

  return (
    <div className={`notification-item fade-in`}>
      <Button
        className={`transparent-button cancel-notification-btn ${
          deleting ? "disabled" : ""
        }`}
        onClick={async (e) => {
          await deleteNotification(e);
        }}
      >
        {!deleting ? (
          <h4>
            <Icon
              name="cancel"
              size="small"
              className="m-0 cancel-notification-icon"
            ></Icon>
          </h4>
        ) : (
          <InPageLoadingComponent width={20} height={20} />
        )}
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
