import { Route } from "react-router-dom";
import "../../App.css";
import TopNavBar from "./TopNavBar";
import { Container } from "semantic-ui-react";
import LandingPage from "../../features/LandingPage/LandingPage";
import Login from "../../features/Authentication/Login";

const App = () => {
  return (
    <div>
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
    </div>
  );
};

export default App;
