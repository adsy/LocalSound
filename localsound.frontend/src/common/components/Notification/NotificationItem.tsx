import { Icon, Image } from "semantic-ui-react";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import placeholderImg from "../../../assets/placeholder.png";
import { Button } from "react-bootstrap";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import {
  handleUpdateDeletingIds,
  handleUpdateNotifications,
} from "../../../app/redux/actions/notificationSlice";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import agent from "../../../api/agent";

interface Props {
  notification: NotificationModel;
  deletingIds: React.MutableRefObject<string[] | undefined>;
  notificationList: React.MutableRefObject<NotificationModel[] | undefined>;
}

const NotificationItem = ({
  notification,
  deletingIds,
  notificationList,
}: Props) => {
  const userData = useSelector((state: State) => state.user.userDetails);
  const [deleting, setDeleting] = useState(false);
  const dispatch = useDispatch();

  const deleteNextNotification = async () => {
    try {
      await agent.Notifications.removeNotification(
        userData?.memberId!,
        notification.notificationId
      );
      var clone = [...notificationList.current!];
      var newList = clone.filter(
        (x) => x.notificationId !== notification.notificationId
      );
      dispatch(handleUpdateNotifications(newList));
      var deletingClone = deletingIds.current!.filter(
        (x) => x !== notification.notificationId
      );
      dispatch(handleUpdateDeletingIds(deletingClone));
    } catch (err: any) {
      // TODO: do something on error
    }
    setDeleting(false);
  };

  const deleteNotification = async (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    setDeleting(true);
    e.stopPropagation();
    dispatch(
      handleUpdateDeletingIds([
        ...deletingIds.current!,
        notification.notificationId,
      ])
    );
    await deleteNextNotification();
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
