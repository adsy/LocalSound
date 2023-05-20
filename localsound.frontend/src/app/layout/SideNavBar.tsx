import { useDispatch, useSelector } from "react-redux";
import { useHistory } from "react-router-dom";
import { Divider } from "semantic-ui-react";
import logo from "../../assets/logo3.png";
import { State } from "../model/redux/state";

const SideNavBar = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const history = useHistory();

  const signOut = () => {
    //do something
  };

  return (
    <>
      {userDetails ? (
        <div id="sidebar" className="d-inline-block w-100">
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
                <div
                  className="sidebar-item"
                  onClick={() => history.push(`/home`)}
                >
                  <span className="align-self-center d-inline-block"></span>
                  <h4 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                    Home
                  </h4>
                </div>
              </div>
              <div
                className="sidebar-item"
                onClick={async () => await signOut()}
              >
                {/* <span className="home-icon align-self-center d-inline-block"></span> */}
                <h4 className="pl-2 sidebar-text mt-0 mb-0 align-self-center">
                  Logout
                </h4>
              </div>
            </div>
          </div>
        </div>
      ) : null}
    </>
  );
};

export default SideNavBar;
