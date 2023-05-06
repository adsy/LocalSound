import { Route } from "react-router-dom";
import "../../App.css";
import TopNavBar from "./TopNavBar";
import { Container } from "semantic-ui-react";
import LandingPage from "../../features/LandingPage/LandingPage";
import Login from "../../features/Authentication/Login";
import ModalContainer from "../../common/modal/ModalContainer";

const App = () => {
  return (
    <>
      <ModalContainer />
      <Container className="app-container">
        <Route exact path="/" component={LandingPage} />
        <Route exact path="/login" component={Login} />
        <Route
          path={"/(.+)"}
          render={() => (
            <div className="masthead">
              <TopNavBar />
            </div>
          )}
        />
      </Container>
    </>
  );
};

export default App;
