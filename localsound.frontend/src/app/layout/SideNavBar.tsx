import { useDispatch, useSelector } from "react-redux";
import { NavLink, useHistory } from "react-router-dom";
import { Divider } from "semantic-ui-react";
import logo from "../../assets/logo4.png";
import { State } from "../model/redux/state";
import { Button } from "react-bootstrap";

const SideNavBar = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);

  return (
    <>
      <div id="sidebar" className="d-inline-block w-100 fade-in">
        <div className="sidebar-contents">
          <div className="d-flex flex-column justify-content-between h-100">
            <div className="">
              <div className="d-flex flex-column justify-content-center align-content-center pt-3">
                <img
                  alt=""
                  src={logo}
                  width="100"
                  height="100"
                  className="d-inline-block align-self-center blur"
                />
              </div>
              <Divider />
              <div></div>
              <NavLink to="/home" className={`sidebar-item`}>
                <span className="home-icon align-self-center d-inline-block"></span>
                <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                  Home
                </h5>
              </NavLink>
              <NavLink to="/test" className={`sidebar-item`}>
                <span className="home-icon align-self-center d-inline-block"></span>
                <h5 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                  Test
                </h5>
              </NavLink>
            </div>
            <div
              className="w-100 d-flex flex-row justify-content-center align-content-center"
              onClick={async () => await signOut()}
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
      {userDetails ? null : null}
    </>
  );
};

export default SideNavBar;
