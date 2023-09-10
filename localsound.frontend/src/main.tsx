import React from "react";
import ReactDOM from "react-dom/client";
import App from "./app/layout/App.tsx";
import "./index.css";
import "semantic-ui-css/semantic.min.css";
import { Provider } from "react-redux";
import { persistor, store } from "./app/redux/store/store.ts";
import { createBrowserHistory } from "history";
import { Router } from "react-router-dom";
import { PersistGate } from "redux-persist/integration/react";
import LoadingComponent from "./common/components/Loading/LoadingComponent.tsx";
import { Wrapper } from "@googlemaps/react-wrapper";
import { store as reduxStore } from "./app/redux/store/store";
import { injectStore as injectStoreIntoApi } from "./api/agent";

injectStoreIntoApi(reduxStore);

export const history = createBrowserHistory();

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  // <React.StrictMode>
  <Provider store={store}>
    <PersistGate
      loading={<LoadingComponent content="Loading app..." />}
      persistor={persistor}
    >
      <Router history={history}>
        <Wrapper apiKey={import.meta.env.VITE_MAPS_KEY} libraries={["places"]}>
          <App />
        </Wrapper>
      </Router>
    </PersistGate>
  </Provider>
  // </React.StrictMode>
);
