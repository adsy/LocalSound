import { useSelector } from "react-redux";
import {
  Redirect,
  Route,
  RouteComponentProps,
  RouteProps,
} from "react-router-dom";
import { State } from "../model/redux/state";

interface Props extends RouteProps {
  component:
    | React.ComponentType<RouteComponentProps<any>>
    | React.ComponentType<any>;
  isAuthPage?: boolean;
}

const AuthenticationRoute = ({
  component: Component,
  isAuthPage = false,
  ...rest
}: Props) => {
  return <Route {...rest} render={(props) => <Component {...props} />} />;
};

export default AuthenticationRoute;
