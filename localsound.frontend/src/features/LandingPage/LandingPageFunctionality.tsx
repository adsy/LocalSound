import { Row } from "react-bootstrap";
import LandingPageSearch from "./LandingPageSearch";
import Carousel from "./Carousel";

const LandingPageFunctionality = () => {
  return (
    <Row>
      <div className="container">
        <LandingPageSearch />
      </div>
      {/* <div className="container">Filter artists based on location ability</div>
      <div className="container">Trending artists based on location?</div>
      <div className="container">Feature</div>
      <div className="container">Feature`</div> */}
    </Row>
  );
};

export default LandingPageFunctionality;
