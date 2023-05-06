import {
  combineReducers,
  configureStore,
  ConfigureStoreOptions,
} from "@reduxjs/toolkit";
import { createLogger } from "redux-logger";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage"; // defaults to localStorage for web
import ApplicationReducer from "../actions/applicationSlice";
import ModalReducer from "../actions/modalSlice";

const middleware = [];

middleware.push(createLogger());

const enhancers = [...middleware];

var rootReducer = combineReducers({
  app: ApplicationReducer,
  modal: ModalReducer,
});

const persistConfig = {
  key: "localSound",
  storage,
};

const persistedReducer = persistReducer(persistConfig, rootReducer);

const storeOptions: ConfigureStoreOptions = {
  reducer: persistedReducer,
  middleware: enhancers,
};

const store = configureStore(storeOptions);

const persistor = persistStore(store);

export { store, persistor };
