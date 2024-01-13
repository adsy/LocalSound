import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import NotificationItem from "./NotificationItem";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import {
  handleHideNotificationContainer,
  handleRemoveNotification,
  handleSaveNotifications,
  handleUpdateDeletingIds,
  handleUpdateNotifications,
} from "../../../app/redux/actions/notificationSlice";
import agent from "../../../api/agent";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useEffect, useRef, useState } from "react";

const NotificationsContainer = () => {
  const notificationData = useSelector((state: State) => state.notifications);
  const userData = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const history = useHistory();
  const dispatch = useDispatch();

  const deletingIds = useRef<string[]>();
  const notificationList = useRef<NotificationModel[]>();
  deletingIds.current = notificationData.deletingIds;
  notificationList.current = notificationData.notificationList;

  const deleteNextNotification = async () => {
    try {
      await agent.Notifications.removeNotification(
        userData?.memberId!,
        notificationData.deletingIds[0]
      );
      var clone = [...notificationList.current!];
      var newList = clone.filter(
        (x) => x.notificationId !== notificationData.deletingIds[0]
      );
      dispatch(handleUpdateNotifications(newList));
      var deletingClone = deletingIds.current!.filter(
        (x) => x !== notificationData.deletingIds[0]
      );
      dispatch(handleUpdateDeletingIds(deletingClone));
    } catch (err: any) {
      // TODO: do something on error
    }
    setDeleting(false);
  };

  useEffect(() => {
    (async () => {
      if (notificationData.deletingIds.length > 0 && !deleting) {
        setDeleting(true);
        await deleteNextNotification();
      }
    })();
  }, [notificationData.deletingIds, deleting]);

  useEffect(() => {
    return () => {
      dispatch(handleUpdateDeletingIds([]));
    };
  }, []);

  const clickNotification = async (notification: NotificationModel) => {
    dispatch(handleHideNotificationContainer());
    try {
      // await agent.Notifications.removeNotification(notification.notificationId);
      dispatch(
        handleRemoveNotification({
          notificationId: notification.notificationId,
        })
      );
    } catch (err: any) {
      //TODO: Do something on error
    }
    if (notification.redirectUrl) {
      history.push(notification.redirectUrl);
    }
  };

  const getMoreNotifications = async (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    e.stopPropagation();
    setLoading(true);
    try {
      var notificationResponse = await agent.Notifications.getMoreNotifications(
        userData?.memberId!
      );
      dispatch(handleSaveNotifications(notificationResponse));
    } catch (err: any) {
      //TODO: do something on error
    }
    setLoading(false);
  };

  return (
    <div
      className={`${
        notificationData.notificationContainerVisible ? "" : "d-none"
      }`}
    >
      <div className="arrow-up fade-in"></div>
      <div id="notifications-container" className="fade-in">
        {!notificationData.initialLoad ? (
          <div
            className="notification-item-container"
            onClick={(e) => e.stopPropagation()}
          >
            <InPageLoadingComponent width={80} height={80} />
          </div>
        ) : notificationData.initialLoad &&
          notificationData.notificationList.length > 0 ? (
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
          </>
        ) : (
          <>
            {!notificationData.canLoadMore ? (
              <div className="notification-empty d-flex flex-row align-items-center justify-content-center">
                <p>You currently have 0 notifications</p>
              </div>
            ) : (
              <>
                <div className="notification-empty d-flex flex-row align-items-center justify-content-center">
                  <p>You're at the end of the list but you still have more!</p>
                </div>
                {!loading ? (
                  <div className="p-2">
                    <Button
                      className="black-button w-100"
                      onClick={async (e) => await getMoreNotifications(e)}
                    >
                      <h4>Load more</h4>
                    </Button>
                  </div>
                ) : (
                  <InPageLoadingComponent width={30} height={30} />
                )}
              </>
            )}
          </>
        )}
      </div>
    </div>
  );
};

export default NotificationsContainer;
