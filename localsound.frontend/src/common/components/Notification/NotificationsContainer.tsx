import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import NotificationItem from "./NotificationItem";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import {
  handleHideNotificationContainer,
  handleRemoveNotification,
  handleSaveNotifications,
} from "../../../app/redux/actions/notificationSlice";
import agent from "../../../api/agent";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useState } from "react";

const NotificationsContainer = () => {
  const notificationData = useSelector((state: State) => state.notifications);
  const userData = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(false);
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

  const getMoreNotifications = async () => {
    setLoading(true);
    try {
      var notificationResponse = await agent.Notifications.getMoreNotifications(
        userData?.memberId!,
        notificationData.page
      );
      dispatch(handleSaveNotifications(notificationResponse));
    } catch (err: any) {
      //TODO: do something on error
    }
    setLoading(false);
  };

  return (
    <>
      <div className="arrow-up fade-in"></div>
      <div id="notifications-container" className="fade-in">
        {notificationData.notificationList.length > 0 ? (
          <>
            {notificationData.notificationList.map((notification, index) => (
              <div
                key={index}
                className={`notification-item-container ${
                  !notification.notificationViewed ? "unviewed" : ""
                }`}
                onClick={async () => await clickNotification(notification)}
              >
                <NotificationItem notification={notification} />
              </div>
            ))}
            {notificationData.canLoadMore ? (
              <>
                {!loading ? (
                  <div className="p-2">
                    <Button
                      className="black-button w-100"
                      onClick={async () => await getMoreNotifications()}
                    >
                      <h4>Load more</h4>
                    </Button>
                  </div>
                ) : (
                  <InPageLoadingComponent width={30} height={30} />
                )}
              </>
            ) : null}
          </>
        ) : (
          <div className="notification-empty">You have no notifications.</div>
        )}
      </div>
    </>
  );
};

export default NotificationsContainer;
