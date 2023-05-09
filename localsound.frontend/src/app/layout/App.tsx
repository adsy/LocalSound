import { Route } from "react-router-dom";
import "../../App.css";
import TopNavBar from "./TopNavBar";
import { Container } from "semantic-ui-react";
import LandingPage from "../../features/LandingPage/LandingPage";
import Login from "../../features/Authentication/Login";
import ModalContainer from "../../common/modal/ModalContainer";
import { useEffect, useLayoutEffect, useRef } from "react";
import { useDispatch } from "react-redux";
import { handleToggleModal } from "../redux/actions/modalSlice";

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
    dispatch(handleToggleModal({ open: false }));
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
