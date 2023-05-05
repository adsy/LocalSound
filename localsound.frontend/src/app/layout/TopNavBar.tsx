import Navbar from "react-bootstrap/Navbar";

const TopNavbar = () => {
  return (
    <>
      <Navbar>
        <div className="d-flex justify-content-between w-100">
          <Navbar.Brand>
            <div className="d-flex flex-row ml-3">
              {/* <img
                alt=""
                src="/logo.svg"
                width="30"
                height="30"
                className="d-inline-block align-top"
              />{" "} */}
              <h3 className="mb-0">LocalSound</h3>
            </div>
          </Navbar.Brand>
        </div>
      </Navbar>
    </>
  );
};

export default TopNavbar;
