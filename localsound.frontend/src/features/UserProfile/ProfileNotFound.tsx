import { Button, Container } from "react-bootstrap";
import { Header, Icon } from "semantic-ui-react";

const ProfileNotFound = () => {
  return (
    <Container className="mt-5 d-flex flex-column align-items-center">
      <Icon circular inverted name="search" className="mt-2 pt-2" size="huge" />
      <Header icon className="pb-2">
        <h1 className="mt-2">
          We couldn't find a profile that matches that url..
        </h1>
        <h3 className="mt-0">
          Please check if you have typed it wrong and try again!
        </h3>
      </Header>
    </Container>
  );
};

export default ProfileNotFound;
