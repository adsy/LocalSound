import { Route } from "react-router-dom";
import "../../App.css";
import LandingPage from "../../features/LandingPage/LandingPage";
import Login from "../../features/Authentication/Login/Login";
import ModalContainer from "../../common/modal/ModalContainer";
import { useEffect, useRef } from "react";
import { useDispatch } from "react-redux";
import { handleResetModal } from "../redux/actions/modalSlice";
import HomePage from "../../features/Home/HomePage";
import TopNavbar from "./TopNavBar";
import PrivateRoute from "./PrivateRoute";
import NotLoggedInRoute from "./NotLoggedInRoute";
import UserProfileSummary from "../../features/UserProfile/UserProfile";
import { Container } from "react-bootstrap";

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
                  <PrivateRoute
                    exact
                    path="/profile"
                    component={UserProfileSummary}
                  />
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
