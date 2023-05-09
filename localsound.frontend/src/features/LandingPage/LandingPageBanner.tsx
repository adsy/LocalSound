import { Button, Row } from "react-bootstrap";
import { useDispatch } from "react-redux";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";

const LandingPageBanner = () => {
  const dispatch = useDispatch();

  const handleAuthenticationRequest = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <div>
            <div>test</div>
            <div>test</div>
            <div>test</div>
          </div>
        ),
        size: "tiny",
      })
    );
  };

  return (
    <Row className="banner mb-3">
      <div className="d-flex flex-column justify-content-between">
        <div className="d-flex flex-row justify-content-between h-100">
          <div className="d-flex flex-column justify-content-end">
            <div className="d-flex flex-row">
              <span className="navbar-logo align-self-center"></span>
              <h2 className="page-title font-bold ml-1 mt-1">LocalSound</h2>
            </div>
          </div>

          <div className="mt-2">
            <Button
              className="login-button mr-2"
              onClick={() => handleAuthenticationRequest()}
            >
              Login
            </Button>
            <Button
              className="blue-button"
              onClick={() => handleAuthenticationRequest()}
            >
              Create account
            </Button>
          </div>
        </div>
      </div>
    </Row>
  );
};
export default LandingPageBanner;
