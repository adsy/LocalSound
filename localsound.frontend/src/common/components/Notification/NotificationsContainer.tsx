import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import NotificationItem from "./NotificationItem";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import {
  handleHideNotificationContainer,
  handleRemoveNotification,
  handleSaveNotifications,
  handleUpdateNotifications,
} from "../../../app/redux/actions/notificationSlice";
import agent from "../../../api/agent";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useEffect, useState } from "react";
import { set } from "lodash";

const NotificationsContainer = () => {
  const notificationData = useSelector((state: State) => state.notifications);
  const userData = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(false);
  const [queuedDeletes, setQueuedDeletes] = useState<string[]>([]);
  const [deletingId, setDeletingId] = useState<string | null>(null);
  const [deleted, setDeleted] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const history = useHistory();
  const dispatch = useDispatch();

  const deleteNextNotification = async () => {
    try {
      await agent.Notifications.removeNotification(
        userData?.memberId!,
        queuedDeletes[0]
      );
      var clone = [...notificationData.notificationList];
      var newList = clone.filter((x) => x.notificationId !== queuedDeletes[0]);
      dispatch(handleUpdateNotifications(newList));
    } catch (err: any) {
      // TODO: do something on error
    }
    setDeleting(false);
  };

  useEffect(() => {
    (async () => {
      if (deletingId) {
        setQueuedDeletes(queuedDeletes.filter((x) => x !== deletingId));
        setDeletingId(null);
      } else if (queuedDeletes.length > 0 && !deleting) {
        setDeleting(true);
        setDeletingId(queuedDeletes[0]);
        await deleteNextNotification();
      }
    })();
  }, [queuedDeletes, deleting, deleted]);

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
                <NotificationItem
                  notification={notification}
                  queuedDeletes={queuedDeletes}
                  setQueuedDeletes={setQueuedDeletes}
                />
              </div>
            ))}
            {notificationData.canLoadMore ? (
              <>
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
            ) : null}
          </>
        ) : (
          <div className="notification-empty d-flex flex-row align-items-center justify-content-center">
            <h4>You currently have no notifications</h4>
          </div>
        )}
      </div>
    </>
  );
};

export default NotificationsContainer;
