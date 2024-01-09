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
import ProfileReducer from "../actions/profileSlice";

const middleware = [];

middleware.push(createLogger());

const enhancers = [...middleware];

var rootReducer = combineReducers({
  app: ApplicationReducer,
  modal: ModalReducer,
  user: UserReducer,
  player: PlayerReducer,
  pageOperation: PageOperationReducer,
  profile: ProfileReducer,
});

const persistConfig = {
  key: "localSound",
  storage,
  blacklist: ["modal", "player", "pageOperation", "app", "profile"],
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

export { store, persistor };
