import { Route, Switch, useHistory } from "react-router-dom";
import "../../App.css";
import LandingPage from "../../features/LandingPage/LandingPage";
import ModalContainer from "../../common/modal/ModalContainer";
import TopNavbar from "./TopNavBar";
import UserProfile from "../../features/UserProfile/Profile";
import { Container } from "react-bootstrap";
import PrivateRoute from "./PrivateRoute";
import HomePage from "../../features/Home/HomePage";
import { useEffect } from "react";
import agent from "../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import {
  handleResetUserState,
  handleSetUserDetails,
} from "../redux/actions/userSlice";
import { handleResetAppState } from "../redux/actions/applicationSlice";
import { UserModel } from "../model/dto/user.model";
import { State } from "../model/redux/state";
import AccountSettings from "../../features/AccountSettings/AccountSettings";
import MusicPlayer from "../../features/MusicPlayer/MusicPlayer";
import BookingsOverview from "../../features/UserProfile/Booking/BookingsOverview";

const App = () => {
  const history = useHistory();
  const dispatch = useDispatch();
  const userDetails = useSelector((state: State) => state.user?.userDetails);
  const player = useSelector((state: State) => state.player);

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
          await checkCurrentUser().then((userDetails: UserModel) => {
            dispatch(handleSetUserDetails(userDetails));
          });
        } catch (err) {
          await signout().finally(() => {
            history.push("/");
            dispatch(handleResetUserState());
            dispatch(handleResetAppState());
          });
        }
      })();
    }
  }, []);

  return (
    <>
      <ModalContainer />
      <div
        id="app-layout"
        className="d-flex flex-column w-100 justify-content-center"
      >
        <Route
          exact
          path="/"
          render={() => (
            <Container>
              <LandingPage />
            </Container>
          )}
        />
        <Route
          path={"/(.+)"}
          render={() => (
            <>
              <TopNavbar />

              <main className="w-100 app-container app-holder">
                <Container>
                  <div className="masthead">
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
                  </div>
                </Container>
              </main>
            </>
          )}
        />
      </div>
      {player.trackId ? <MusicPlayer /> : null}
    </>
  );
};

export default App;
