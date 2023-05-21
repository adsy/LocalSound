import Navbar from "react-bootstrap/Navbar";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../model/redux/state";
import logo from "../../assets/logo4.png";
import { useState } from "react";
import { Button, Nav, Offcanvas } from "react-bootstrap";
import { Container } from "semantic-ui-react";
import { useHistory } from "react-router-dom";
import { handleToggleModal } from "../redux/actions/modalSlice";
import Login from "../../features/Authentication/Login/Login";
import Register from "../../features/Authentication/Register/Register";

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
              <Offcanvas.Header closeButton>
                <Offcanvas.Title
                  id={`offcanvasNavbarLabel-expand-false`}
                  className="p-3 pb-0 d-flex flex-row"
                >
                  <img
                    alt=""
                    src={logo}
                    width="50"
                    height="50"
                    className="d-inline-block align-top"
                  />
                  <h4 className="navbar-title m-0 pl-2 align-self-center">
                    Settings
                  </h4>
                </Offcanvas.Title>
              </Offcanvas.Header>
              <Offcanvas.Body>
                <Nav className=" flex-grow-1 pe-4 p-3">
                  {/* <>{createUserRelevantLinkItems()}</>
                <Nav.Link href="#" onClick={() => handleHomeLinkClick()}>
                  <div className="d-flex flex-row align-content-center">
                    <div className="house-icon-nav align-self-center mr-2"></div>
                    <h4 className="mt-0 align-self-center">Home</h4>
                  </div>
                </Nav.Link>
                <Nav.Link href="#action1">
                  <div className="d-flex flex-row align-content-center">
                    <div className="settings-icon-nav align-self-center mr-2"></div>
                    <h4 className="mt-0 align-self-center">Settings</h4>
                  </div>
                </Nav.Link>
                <Nav.Link href="#action1">
                  <div className="d-flex flex-row align-content-center">
                    <div className="phone-icon-nav align-self-center mr-2"></div>
                    <h4 className="mt-0 align-self-center">Contact us</h4>
                  </div>
                </Nav.Link>
                <Nav.Link href="#action1">
                  <div className="d-flex flex-row align-content-center">
                    <div className="ask-icon-nav align-self-center mr-2"></div>
                    <h4 className="mt-0 align-self-center">FAQ</h4>
                  </div>
                </Nav.Link>
                <div className="d-flex nav-bar-logout">
                  <Button
                    onClick={async () => {
                      signOut();
                    }}
                    variant=""
                    className="purple-btn fade-in w-100 align-self-end mt-4"
                  >
                    Logout
                  </Button>
                </div> */}
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
