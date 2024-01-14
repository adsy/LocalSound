import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import NotificationItem from "./NotificationItem";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import {
  handleHideNotificationContainer,
  handleSaveNotifications,
  handleUpdateNotificationToViewed,
} from "../../../app/redux/actions/notificationSlice";
import agent from "../../../api/agent";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useState } from "react";
import InfiniteScroll from "react-infinite-scroll-component";

const NotificationsContainer = () => {
  const notificationData = useSelector((state: State) => state.notifications);
  const userData = useSelector((state: State) => state.user.userDetails);
  const [page, setPage] = useState(0);
  const history = useHistory();
  const dispatch = useDispatch();

  const clickNotification = async (notification: NotificationModel) => {
    try {
      if (!notification.notificationViewed) {
        agent.Notifications.clickNotification(
          userData?.memberId!,
          notification.notificationId
        );

        dispatch(handleUpdateNotificationToViewed(notification));
      }

      if (notification.redirectUrl) {
        history.push(notification.redirectUrl);
      }
    } catch (err: any) {
      //TODO: Do something on error
    }
    dispatch(handleHideNotificationContainer());
  };

  const getMoreNotifications = async () => {
    try {
      var notificationResponse = await agent.Notifications.getMoreNotifications(
        userData?.memberId!,
        page + 1
      );
      dispatch(handleSaveNotifications(notificationResponse));
    } catch (err: any) {
      //TODO: do something on error
    }
    setPage(page + 1);
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
            <InfiniteScroll
              dataLength={notificationData.notificationList.length} //This is important field to render the next data
              next={() => getMoreNotifications()}
              hasMore={notificationData.canLoadMore}
              loader={
                <InPageLoadingComponent
                  withContainer={true}
                  width={50}
                  height={50}
                  containerClass="mt-0 br-0 pt-1 pb-1"
                />
              }
              scrollableTarget={"notifications-container"}
            >
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
            </InfiniteScroll>
          </>
        ) : (
          <>
            <div className=" fade-in notification-empty d-flex flex-row align-items-center justify-content-center">
              <p>You currently have 0 notifications...</p>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default NotificationsContainer;
