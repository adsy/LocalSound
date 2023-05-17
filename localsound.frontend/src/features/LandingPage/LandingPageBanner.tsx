import { Button, Row } from "react-bootstrap";
import { useDispatch } from "react-redux";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import Login from "../Authentication/Login/Login";
import Register from "../Authentication/Register/Register";

const LandingPageBanner = () => {
  const dispatch = useDispatch();

  const handleAuthenticationRequest = (isLogin: boolean) => {
    if (isLogin) {
      dispatch(
        handleToggleModal({
          open: true,
          body: <Login />,
          size: "tiny",
        })
      );
    } else {
      dispatch(
        handleToggleModal({
          open: true,
          body: <Register />,
          size: "tiny",
        })
      );
    }
  };

  return (
    <Row className="banner mb-4">
      <div className="d-flex flex-column justify-content-between w-100">
        <div className="d-flex flex-row justify-content-between flex-wrap">
          <div className="d-flex flex-row justify-content-start mb-2">
            <span className="navbar-logo align-self-center"></span>
            <h2 className="page-title font-bold ml-1 mt-1">LocalSound</h2>
          </div>

          <div className="justify-content-end mt-2">
            <Button
              className="purple-button mr-2"
              onClick={() => handleAuthenticationRequest(true)}
            >
              Login
            </Button>
            <Button
              className="purple-button"
              onClick={() => handleAuthenticationRequest(false)}
            >
              Create account
            </Button>
          </div>
        </div>
        <div
          id="landing-page-search"
          className="d-flex flex-column justify-content-start mb-2"
        >
          <p className="text-center search-text">
            Better Soundtracking than your Cousin's Dubstep mix.
          </p>
          <div className="d-flex justify-content-center">
            <Button className="purple-button w-50">
              Search for your next performer
            </Button>
          </div>
        </div>
      </div>
    </Row>
  );
};
export default LandingPageBanner;
