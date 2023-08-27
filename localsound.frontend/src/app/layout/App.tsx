import { Route, Switch } from "react-router-dom";
import "../../App.css";
import LandingPage from "../../features/LandingPage/LandingPage";
import ModalContainer from "../../common/modal/ModalContainer";
import TopNavbar from "./TopNavBar";
import UserProfile from "../../features/UserProfile/Profile";
import { Container } from "react-bootstrap";
import PrivateRoute from "./PrivateRoute";
import HomePage from "../../features/Home/HomePage";

const App = () => {
  return (
    <>
      <ModalContainer />
      <div
        id="app-layout"
        className="d-flex flex-column w-100 justify-content-center"
      >
        <TopNavbar />
        <div className="d-flex flex-row w-100 justify-content-center app-holder">
          <Container className="app-container">
            <Route exact path="/" component={LandingPage} />
            <Route
              path={"/(.+)"}
              render={() => (
                <div className="masthead">
                  <Switch>
                    <PrivateRoute exact path="/home" component={HomePage} />
                    <Route exact path="/(.+)" component={UserProfile} />
                  </Switch>
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
