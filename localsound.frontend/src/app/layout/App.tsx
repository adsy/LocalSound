import { Route } from "react-router-dom";
import "../../App.css";
import TopNavBar from "./TopNavBar";
import { Container } from "semantic-ui-react";
import LandingPage from "../../features/LandingPage/LandingPage";
import Login from "../../features/Authentication/Login/Login";
import ModalContainer from "../../common/modal/ModalContainer";
import { useEffect, useLayoutEffect, useRef } from "react";
import { useDispatch } from "react-redux";
import {
  handleResetModal,
  handleToggleModal,
} from "../redux/actions/modalSlice";
import HomePage from "../../features/Home/HomePage";
import SideNavBar from "./SideNavBar";
import TopNavbar from "./TopNavBar";
import PrivateRoute from "./PrivateRoute";
import NotLoggedInRoute from "./NotLoggedInRoute";

const App = () => {
  const dispatch = useDispatch();

  const useUnload = (fn: any) => {
    const cb = useRef(fn);

    useEffect(() => {
      const onUnload = cb.current;
      window.addEventListener("beforeunload", onUnload);
      return () => {
        window.removeEventListener("beforeunload", onUnload);
      };
    }, [cb]);
  };

  useUnload((e: any) => {
    e.preventDefault();
    // Reset modal to default for when page is next reloaded with persisted state
    dispatch(handleResetModal());
  });

  return (
    <>
      <ModalContainer />
      <div
        id="app-layout"
        className="d-flex flex-column w-100 justify-content-center"
      >
        <TopNavbar />
        <div className="d-flex flex-row w-100 justify-content-center app-holder">
          {/* <SideNavBar /> */}
          <Container className="app-container">
            <NotLoggedInRoute exact path="/" component={LandingPage} />
            <NotLoggedInRoute exact path="/login" component={Login} />
            <Route
              path={"/(.+)"}
              render={() => (
                <div className="masthead">
                  <PrivateRoute exact path="/home" component={HomePage} />
                </div>
              )}
            />
          </Container>
        </div>
      </div>
    </>
  );
};

export default App;
