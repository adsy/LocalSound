import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { toast } from "react-toastify";
import { NotificationModel } from "../app/model/dto/notification.model";
import {
  StoreSaveNotification,
  StoreSaveNotifications,
} from "../app/redux/store/store";
import NotificationPopUp from "../common/components/Notification/NotificationPopUp";

var hubConnection: HubConnection | null;

const returnNotification = (notification: NotificationModel) => {
  return <NotificationPopUp notification={notification} />;
};

const createSignalConnection = () => {
  hubConnection = new HubConnectionBuilder()
    .withUrl(import.meta.env.VITE_NOTIFICATION_URL!, {
      withCredentials: true,
    })
    .withAutomaticReconnect()
    .configureLogging(LogLevel.Information)
    .build();

  hubConnection
    .start()
    .catch((error) =>
      console.log("Error establishing signalR connection: ", error)
    );

  hubConnection.on(
    "LoadNotifications",
    (notifications: NotificationModel[]) => {
      StoreSaveNotifications(notifications);
    }
  );

  hubConnection.on("ReceiveNotification", (notification: NotificationModel) => {
    StoreSaveNotification(notification);
    toast(returnNotification(notification));
  });
};

const disconnectConnection = async () => {
  await hubConnection?.stop();
};

const signalHub = {
  createSignalConnection,
  disconnectConnection,
};

export default signalHub;
