import { Route, Switch, useHistory } from "react-router-dom";
import "../../App.css";
import LandingPage from "../../features/LandingPage/LandingPage";
import ModalContainer from "../../common/modal/ModalContainer";
import UserProfile from "../../features/UserProfile/Profile";
import { Container } from "react-bootstrap";
import PrivateRoute from "./PrivateRoute";
import HomePage from "../../features/Home/HomePage";
import { useEffect, useRef } from "react";
import agent from "../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import {
  handleResetUserState,
  handleSetUserDetails,
} from "../redux/actions/userSlice";
import {
  handleAppLoading,
  handleResetAppState,
} from "../redux/actions/applicationSlice";
import { UserModel } from "../model/dto/user.model";
import { State } from "../model/redux/state";
import AccountSettings from "../../features/AccountSettings/AccountSettings";
import MusicPlayer from "../../features/MusicPlayer/MusicPlayer";
import BookingsOverview from "../../features/UserProfile/Booking/BookingsOverview";
import InPageLoadingComponent from "./InPageLoadingComponent";
import signalHub from "../../api/signalR";
import {
  handleHideNotificationContainer,
  handleResetNotificationState,
} from "../redux/actions/notificationSlice";
import TopNavbar from "./TopNavBar";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const App = () => {
  const history = useHistory();
  const dispatch = useDispatch();
  const userDetails = useSelector((state: State) => state.user?.userDetails);
  const appLoading = useSelector((state: State) => state.app.appLoading);
  const notificationData = useSelector((state: State) => state.notifications);
  const player = useSelector((state: State) => state.player);

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
    // disconnect from signalR when page is closed
    signalHub.disconnectConnection();
    dispatch(handleResetNotificationState());
  });

  useEffect(() => {
    if (userDetails) {
      const checkCurrentUser = async () => {
        return await agent.Authentication.checkCurrentUser();
      };

      const signout = async () => {
        await agent.Authentication.signOut();
      };

      (async () => {
        try {
          dispatch(handleAppLoading(true));
          await checkCurrentUser().then((userDetails: UserModel) => {
            dispatch(handleSetUserDetails(userDetails));
            dispatch(handleAppLoading(false));
          });
        } catch (err) {
          await signout().finally(() => {
            history.push("/");
            dispatch(handleResetUserState());
            dispatch(handleResetAppState());
          });
        }
      })();
    } else {
      dispatch(handleAppLoading(false));
    }
  }, []);

  useEffect(() => {
    if (userDetails) {
      signalHub.createSignalConnection();
    }
  }, [userDetails?.memberId]);

  return (
    <>
      <ModalContainer />

      <Route
        exact
        path="/"
        render={() => (
          <div id="app-layout" className="d-flex flex-column w-100">
            <Container>
              <LandingPage />
            </Container>
          </div>
        )}
      />
      <div
        id="app-layout"
        className="d-flex flex-column w-100 justify-content-center"
        onClick={() => {
          if (notificationData.notificationContainerVisible) {
            dispatch(handleHideNotificationContainer());
          }
        }}
      >
        <Route
          path={"/(.+)"}
          render={() => (
            <>
              <TopNavbar />
              {appLoading ? (
                <div className="align-self-center d-flex justify-content-center">
                  <InPageLoadingComponent
                    height={100}
                    width={100}
                    withContainer={true}
                  />
                </div>
              ) : (
                <main className={`w-100 app-container app-holder d-flex`}>
                  <Container>
                    <Switch>
                      <PrivateRoute exact path="/home" component={HomePage} />
                      <PrivateRoute
                        exact
                        path="/bookings"
                        component={BookingsOverview}
                      />
                      <PrivateRoute
                        exact
                        path="/account-settings"
                        component={AccountSettings}
                      />
                      <Route exact path="/(.+)" component={UserProfile} />
                    </Switch>
                  </Container>
                </main>
              )}
              <ToastContainer
                position="bottom-right"
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="dark"
              />
            </>
          )}
        />
      </div>
      {player.currentSong ? <MusicPlayer /> : null}
    </>
  );
};

export default App;
