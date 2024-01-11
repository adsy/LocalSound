import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import NotificationItem from "./NotificationItem";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import {
  handleHideNotificationContainer,
  handleRemoveNotification,
} from "../../../app/redux/actions/notificationSlice";
import agent from "../../../api/agent";

const NotificationsContainer = () => {
  const notifications = useSelector(
    (state: State) => state.notifications.notificationList
  );
  const history = useHistory();
  const dispatch = useDispatch();

  const clickNotification = async (notification: NotificationModel) => {
    dispatch(handleHideNotificationContainer());
    try {
      await agent.Notifications.removeNotification(notification.notificationId);
      dispatch(
        handleRemoveNotification({
          notificationId: notification.notificationId,
        })
      );
    } catch (err: any) {
      //TODO: Do something on error
    }
    history.push(notification.redirectUrl);
  };

  return (
    <div id="notifications-container">
      {notifications.length > 0 ? (
        <>
          {notifications.map((notification, index) => (
            <div
              className="notification-item"
              onClick={async () => await clickNotification(notification)}
            >
              <NotificationItem notification={notification} />
            </div>
          ))}
        </>
      ) : (
        <div>You have no notifications.</div>
      )}
    </div>
  );
};

export default NotificationsContainer;
