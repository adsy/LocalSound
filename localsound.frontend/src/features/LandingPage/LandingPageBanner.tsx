import { Button, Row } from "react-bootstrap";

const LandingPageBanner = () => {
  return (
    <Row className="banner mb-4">
      <div className="d-flex flex-column justify-content-between w-100">
        <div className="d-flex flex-row justify-content-center h-100 align-content-center flex-wrap">
          <div className="d-flex flex-row justify-content-center align-content-center mb-2">
            <span className="landing-page-logo align-self-center"></span>
            {/* <h2 className="page-title font-bold ml-1 mt-1">LocalSound</h2> */}
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
            <Button className="black-button w-50">
              Search for your next performer
            </Button>
          </div>
        </div>
      </div>
    </Row>
  );
};
export default LandingPageBanner;
