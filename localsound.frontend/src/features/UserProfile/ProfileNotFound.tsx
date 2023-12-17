import { Button, Container } from "react-bootstrap";
import logo from "../../assets/updated-logo4.png";
import { Header, Icon } from "semantic-ui-react";

const ProfileNotFound = () => {
  return (
    <Container className="d-flex flex-column align-items-center justify-content-center">
      <div className="d-flex flex-column align-items-center justiify-content-center component-container pl-5 pr-5">
        <img
          alt=""
          src={logo}
          width="150"
          height="150"
          className="d-inline-block align-top"
        />
        <Header icon className="pb-2">
          <h2 className="mt-2">
            We couldn't find a profile that matches that url.
          </h2>
          <h4 className="mt-0">
            Please check if you have typed it wrong and try again!
          </h4>
        </Header>
      </div>
    </Container>
  );
};

export default ProfileNotFound;
