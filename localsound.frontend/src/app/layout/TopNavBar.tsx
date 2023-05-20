import Navbar from "react-bootstrap/Navbar";
import { useSelector } from "react-redux";
import { State } from "../model/redux/state";
import logo from "../../assets/logo2.png";

const TopNavbar = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);

  return (
    <>
      {userDetails ? (
        <Navbar>
          <div className="d-flex justify-content-between w-100">
            <Navbar.Brand>
              <div className="d-flex flex-row justify-content-center align-content-center ml-3">
                <img
                  alt=""
                  src={logo}
                  width="50"
                  height="50"
                  className="d-inline-block align-top"
                />
                {/* <h1 className="navbar-title mt-0 mb-0">LocalSound</h1> */}
              </div>
            </Navbar.Brand>
          </div>
        </Navbar>
      ) : null}
    </>
  );
};

export default TopNavbar;
