import { useDispatch, useSelector } from "react-redux";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";
import userImg from "../../../assets/placeholder.png";
import agent from "../../../api/agent";
import { State } from "../../../app/model/redux/state";
import { handleUpdateNotificationToViewed } from "../../../app/redux/actions/notificationSlice";

interface Props {
  notification: NotificationModel;
}

const NotificationPopUp = ({ notification }: Props) => {
  const userData = useSelector((state: State) => state.user.userDetails);
  const history = useHistory();
  const dispatch = useDispatch();

  const clickNotification = async () => {
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
      // do nothing on this error api error
    }
  };

  return (
    <div
      className={`px-2 py-1 my-2 notification`}
      onClick={async () => {
        await clickNotification();
      }}
    >
      <div className="d-flex flex-row align-items-center clickable">
        <img
          className="notification-image mr-1"
          width={30}
          height={30}
          src={notification.userImage ? notification.userImage : userImg}
        />
        <div className="font-bold p-1 notification-text">
          {notification.notificationMessage}
        </div>
      </div>
    </div>
  );
};

export default NotificationPopUp;
