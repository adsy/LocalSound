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
  StoreUpdateNotificationInitialLoad,
} from "../app/redux/store/store";
import NotificationPopUp from "../common/components/Notification/NotificationPopUp";
import { CreateNotification } from "../app/model/dto/create-notification.model";
import { NotificationListResponseModel } from "../app/model/dto/notification-list-response.model";

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
    (notifications: NotificationListResponseModel) => {
      StoreSaveNotifications(notifications);
      StoreUpdateNotificationInitialLoad();
    }
  );

  hubConnection.on("ReceiveNotification", (notification: NotificationModel) => {
    StoreSaveNotification(notification);
    toast(returnNotification(notification));
  });
};

const createNotification = async (payload: CreateNotification) => {
  try {
    await hubConnection?.invoke("CreateNotification", payload);
  } catch (e) {
    console.log(e);
  }
};

const disconnectConnection = async () => {
  await hubConnection?.stop();
};

const signalHub = {
  createSignalConnection,
  disconnectConnection,
  createNotification,
};

export default signalHub;
