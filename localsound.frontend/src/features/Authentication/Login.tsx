import { Button } from "react-bootstrap";

const Login = () => {
  return (
    <div id="auth-modal">
      <div className="d-flex flex-row mb-2 header">
        <h2 className="header-title ml-1 mt-1 align-self-center">
          Login to your account
        </h2>
      </div>
      <div className="d-flex header justify-content-end mt-2">
        <Button className="purple-button mr-2">I play the music</Button>
        <Button className="purple-button">I listen to the music</Button>
      </div>
    </div>
  );
};

export default Login;
