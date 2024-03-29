import { useSelector } from "react-redux";
import {
  Redirect,
  Route,
  RouteComponentProps,
  RouteProps,
} from "react-router-dom";
import { State } from "../model/redux/state";
import { CustomerTypes } from "../model/enums/customerTypes";

interface Props extends RouteProps {
  component:
    | React.ComponentType<RouteComponentProps<any>>
    | React.ComponentType<any>;
  allowedCustomerType?: CustomerTypes;
}

const PrivateRoute = ({
  component: Component,
  allowedCustomerType,
  ...rest
}: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);

  if (userDetails && userDetails.emailConfirmed && allowedCustomerType) {
    if (allowedCustomerType !== userDetails.customerType) {
      return <Redirect to={"/home"} />;
    }
  } else if (!userDetails || !userDetails.emailConfirmed) {
    return <Redirect to={"/"} />;
  }

  return <Route {...rest} render={(props) => <Component {...props} />} />;
};

export default PrivateRoute;
