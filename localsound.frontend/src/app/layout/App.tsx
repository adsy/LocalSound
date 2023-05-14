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
