import { Button } from "react-bootstrap";

const Login = () => {
  return (
    <div>
      Login page
      <div className="mr-3 nav-buttons">
        <Button className="login-button mr-2">Login</Button>
        <Button className="signup-button">Create account</Button>
      </div>
    </div>
  );
};

export default Login;
