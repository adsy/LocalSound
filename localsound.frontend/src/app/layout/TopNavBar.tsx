import Navbar from "react-bootstrap/Navbar";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../model/redux/state";
import logo from "../../assets/logo4.png";
import { useState } from "react";
import { Button, Nav, Offcanvas } from "react-bootstrap";
import { Container, Divider } from "semantic-ui-react";
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
    await agent.Authentication.signOut();
    dispatch(handleResetUserState());
    dispatch(handleResetAppState());
  };

  return (
    <>
      <div id="navbar" className="w-100">
        <Container>
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
                />
              ) : (
                <div className="justify-content-end ml-3">
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
              )}
            </div>
            <Navbar.Offcanvas
              id={`offcanvasNavbar-expand-false`}
              aria-labelledby={`offcanvasNavbarLabel-expand-false`}
              placement="end"
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
                            <NavLink to="/home" className={`sidebar-item mb-2`}>
                              <span className="home-icon align-self-center d-inline-block"></span>
                              <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                                Home
                              </h5>
                            </NavLink>
                            <NavLink to="/test" className={`sidebar-item mb-2`}>
                              <span className="home-icon align-self-center d-inline-block"></span>
                              <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                                Test
                              </h5>
                            </NavLink>
                          </div>
                          <div
                            className="w-100 d-flex flex-row justify-content-center align-content-center"
                            onClick={async () => await handleSignout()}
                          >
                            <Button className="purple-button d-flex flex-row justify-content-center mb-2 w-100 mx-5">
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
