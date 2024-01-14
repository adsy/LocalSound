import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";

const UnreadNotificationCount = () => {
  const unreadCount = useSelector(
    (state: State) => state.notifications.unreadNotifications
  );

  return (
    <div className="unread-count fade-in">
      <h4 className="mb-0">{unreadCount > 99 ? "99+" : unreadCount}</h4>
    </div>
  );
};

export default UnreadNotificationCount;
