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
  allowedCustomerType?: CustomerTypes;
}

const NotLoggedInRoute = ({
  component: Component,
  allowedCustomerType,
  ...rest
}: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);

  if (userDetails) {
    return <Redirect to={"/home"} />;
  }

  return <Route {...rest} render={(props) => <Component {...props} />} />;
};

export default NotLoggedInRoute;
