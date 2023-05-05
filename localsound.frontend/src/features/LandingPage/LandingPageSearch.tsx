import { Button, Row } from "react-bootstrap";

const LandingPageSearch = () => {
  return (
    <div
      id="landing-page-search"
      className="h-100 d-flex justify-content-center align-items-center mb-4"
    >
      <div>
        <p className="text-center search-text">
          Text goes here how you can search for an artist / DJ / performer
          within your area for your next party so you dont need to rely on your
          friends cousins girlfriends nephew to do it.
        </p>
        <div className="d-flex justify-content-center">
          <Button className="clear-black-button w-50">
            Search for your next performer
          </Button>
        </div>
      </div>
    </div>
  );
};
export default LandingPageSearch;
