import { Route } from "react-router-dom";
import "../../App.css";
import LandingPageContainer from "../../common/components/LandingPage/LandingPageContainer";
import TopNavBar from "./TopNavBar";
import { Container } from "semantic-ui-react";

const App = () => {
  return (
    <div>
      <Container>
        <Route exact path="/" component={LandingPageContainer} />
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
