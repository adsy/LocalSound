import React from "react";
import ReactDOM from "react-dom/client";
import App from "./app/layout/App.tsx";
import "./index.css";
import "semantic-ui-css/semantic.min.css";
import { Provider } from "react-redux";
import { persistor, store } from "./app/redux/store/store.ts";
import { Router } from "react-router-dom";
import { PersistGate } from "redux-persist/integration/react";
import LoadingComponent from "./common/components/Loading/LoadingComponent.tsx";
import { Wrapper } from "@googlemaps/react-wrapper";
import { history } from "./common/history/history.ts";

// Create the script tag, set the appropriate attributes
var script = document.createElement("script");
script.src = `https://maps.googleapis.com/maps/api/js?key=${
  import.meta.env.VITE_MAPS_KEY
}&callback=initMap&libraries=places&loading=async`;
script.async = true;

// Attach your callback function to the `window` object
window.initMap = function () {
  // JS API is loaded and available
};

// Append the 'script' element to 'head'
document.head.appendChild(script);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  // <React.StrictMode>
  <Provider store={store}>
    <PersistGate
      loading={<LoadingComponent content="Loading app..." />}
      persistor={persistor}
    >
      <Router history={history}>
        {/* <Wrapper apiKey={import.meta.env.VITE_MAPS_KEY} libraries={["places"]}> */}
        <App />
        {/* </Wrapper> */}
      </Router>
    </PersistGate>
  </Provider>
  // </React.StrictMode>
);
