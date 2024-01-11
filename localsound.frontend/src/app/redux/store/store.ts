import {
  combineReducers,
  configureStore,
  ConfigureStoreOptions,
} from "@reduxjs/toolkit";
import { createLogger } from "redux-logger";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage"; // defaults to localStorage for web
import ApplicationReducer, {
  handleResetAppState,
} from "../actions/applicationSlice";
import ModalReducer from "../actions/modalSlice";
import PlayerReducer from "../actions/playerSlice";
import UserReducer, { handleResetUserState } from "../actions/userSlice";
import PageOperationReducer from "../actions/pageOperationSlice";
import PageDataReducer from "../actions/pageDataSlice";
import NotificationReducer, {
  handleHideNotificationContainer,
  handleSaveNotification,
  handleSaveNotifications,
} from "../actions/notificationSlice";
import { NotificationModel } from "../../model/dto/notification.model";
import { NotificationListResponseModel } from "../../model/dto/notification-list-response.model";

const middleware = [];

middleware.push(createLogger());

const enhancers = [...middleware];

var rootReducer = combineReducers({
  app: ApplicationReducer,
  modal: ModalReducer,
  user: UserReducer,
  player: PlayerReducer,
  pageOperation: PageOperationReducer,
  pageData: PageDataReducer,
  notifications: NotificationReducer,
});

const persistConfig = {
  key: "localSound",
  storage,
  blacklist: [
    "modal",
    "player",
    "pageOperation",
    "app",
    "pageData",
    "notifications",
  ],
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

const storeOptions: ConfigureStoreOptions = {
  reducer: persistedReducer,
  middleware: enhancers,
};

const store = configureStore(storeOptions);

const persistor = persistStore(store);

export const resetState = () => {
  store.dispatch(handleResetUserState());
  store.dispatch(handleResetAppState());
};

export const StoreSaveNotifications = (
  notifications: NotificationListResponseModel
) => {
  store.dispatch(handleSaveNotifications(notifications));
};

export const StoreSaveNotification = (notification: NotificationModel) => {
  store.dispatch(handleSaveNotification(notification));
};

export { store, persistor };
