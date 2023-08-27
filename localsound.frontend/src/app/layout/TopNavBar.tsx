import Navbar from "react-bootstrap/Navbar";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../model/redux/state";
import logo from "../../assets/logo4.png";
import { useState } from "react";
import { Button, Container, Nav, Offcanvas } from "react-bootstrap";
import { Divider, Icon } from "semantic-ui-react";
import { NavLink, useHistory } from "react-router-dom";
import { handleToggleModal } from "../redux/actions/modalSlice";
import Login from "../../features/Authentication/Login/Login";
import Register from "../../features/Authentication/Register/Register";
import agent from "../../api/agent";
import { handleResetUserState } from "../redux/actions/userSlice";
import { handleResetAppState } from "../redux/actions/applicationSlice";

const TopNavbar = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const history = useHistory();
  const dispatch = useDispatch();

  const [show, setShow] = useState(false);

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

  const handleSignout = async () => {
    try {
      var result = await agent.Authentication.signOut();
      dispatch(handleResetUserState());
      dispatch(handleResetAppState());
      setShow(false);
      history.push("/");
    } catch (error) {
      //TODO: do something with error
    }
  };

  return (
    <>
      <div id="navbar" className="w-100 mb-4">
        <Container className="pl-1">
          <Navbar collapseOnSelect key={null} bg="" expand={false}>
            <div
              className="navbar-logo d-flex flex-row justify-content-center align-content-center pr-2 pt-1 pb-1"
              onClick={() => {
                if (userDetails) {
                  history.push("/home");
                } else {
                  history.push("/");
                }
              }}
            >
              <img
                alt=""
                src={logo}
                width="50"
                height="50"
                className="d-inline-block align-top"
              />
              <h4 className="navbar-title m-0 pl-2 align-self-center">
                LocalSound
              </h4>
            </div>
            <div className="d-flex flex-row ml-1">
              {userDetails ? (
                <Navbar.Toggle
                  aria-controls={`offcanvasNavbar-expand-false`}
                  className=" mr-1 align-self-center"
                  onClick={() => setShow(true)}
                />
              ) : (
                <div className="justify-content-end ml-3">
                  <Button
                    className="black-button mr-2"
                    onClick={() => handleAuthenticationRequest(true)}
                  >
                    <h4>Login</h4>
                  </Button>
                  <Button
                    className="black-button"
                    onClick={() => handleAuthenticationRequest(false)}
                  >
                    <h4>Create account</h4>
                  </Button>
                </div>
              )}
            </div>
            <Navbar.Offcanvas
              id={`offcanvasNavbar-expand-false`}
              aria-labelledby={`offcanvasNavbarLabel-expand-false`}
              placement="end"
              show={show}
              onHide={() => setShow(false)}
            >
              <Offcanvas.Header closeButton className="pb-0"></Offcanvas.Header>
              <Offcanvas.Body>
                <Nav className=" flex-grow-1 h-100">
                  <div className="d-inline-block w-100 fade-in h-100">
                    <div className="sidebar-contents h-100">
                      <div className="d-flex flex-column justify-content-between h-100">
                        <div className="">
                          <div className="d-flex flex-column justify-content-center align-content-center pt-3">
                            <img
                              alt=""
                              src={logo}
                              width="150"
                              height="150"
                              className="d-inline-block align-self-center blur"
                            />
                          </div>
                          <Divider />
                        </div>
                        <div className="d-flex flex-column justify-content-between h-100 sidebar-link-container p-3">
                          <div>
                            <NavLink
                              to="/home"
                              className={`sidebar-item mb-2`}
                              onClick={() => setShow(false)}
                            >
                              <Icon name="home" size="large" />
                              <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                                Home
                              </h5>
                            </NavLink>
                            <NavLink
                              to={`/${userDetails?.profileUrl}`}
                              className={`sidebar-item mb-2`}
                              onClick={() => setShow(false)}
                            >
                              <Icon name="user" size="large" />
                              <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                                Profile
                              </h5>
                            </NavLink>
                            <NavLink
                              to={`/account-settings`}
                              className={`sidebar-item mb-2`}
                              onClick={() => setShow(false)}
                            >
                              <Icon name="settings" size="large" />
                              <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                                Account settings
                              </h5>
                            </NavLink>
                          </div>
                          <div
                            className="w-100 d-flex flex-row justify-content-center align-content-center"
                            onClick={async () => await handleSignout()}
                          >
                            <Button className="black-button d-flex flex-row justify-content-center mb-2 w-100 mx-5">
                              <span className="signout-icon align-self-center d-inline-block"></span>
                              <h4 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                                Logout
                              </h4>
                            </Button>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </Nav>
              </Offcanvas.Body>
            </Navbar.Offcanvas>
          </Navbar>
        </Container>
      </div>
    </>
  );
};

export default TopNavbar;
