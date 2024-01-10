import { useDispatch } from "react-redux";
import { useHistory } from "react-router-dom";
import { NotificationModel } from "../../../app/model/dto/notification.model";

interface Props {
  notification: NotificationModel;
}

const NotificationPopUp = ({ notification }: Props) => {
  const history = useHistory();
  const dispatch = useDispatch();

  const clickNotification = async () => {
    history.push(notification.redirectUrl);
    // await agent.Notification.clickNotification(
    //   notification.notificationId
    // ).then(() => {
    //   dispatch(handleRemoveNotification(notification.notificationId));
    //   dispatch(handleSelectStore(notification.storeId));
    // });
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
          width={50}
          height={50}
          src={notification.userImage}
        />
        <div className="font-bold p-1 notification-text">
          {notification.notificationMessage}
        </div>
      </div>
    </div>
  );
};

export default NotificationPopUp;
