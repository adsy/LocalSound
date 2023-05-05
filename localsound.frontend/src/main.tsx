import React from "react";
import ReactDOM from "react-dom/client";
import App from "./app/layout/App.tsx";
import "./index.css";
import { Provider } from "react-redux";
import { persistor, store } from "./app/redux/store/store.ts";
import { createBrowserHistory } from "history";
import { Router } from "react-router-dom";
import { PersistGate } from "redux-persist/integration/react";
import LoadingComponent from "./common/components/Loading/LoadingComponent.tsx";

export const history = createBrowserHistory();

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <Provider store={store}>
      <PersistGate
        loading={<LoadingComponent content="Loading app..." />}
        persistor={persistor}
      >
        <Router history={history}>
          <App />
        </Router>
      </PersistGate>
    </Provider>
  </React.StrictMode>
);
