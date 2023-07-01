import { Route, RouteComponentProps, RouteProps } from "react-router-dom";

interface Props extends RouteProps {
  component:
    | React.ComponentType<RouteComponentProps<any>>
    | React.ComponentType<any>;
  isAuthPage?: boolean;
}

const NonPrivateRoute = ({
  component: Component,
  isAuthPage = false,
  ...rest
}: Props) => {
  return <Route {...rest} render={(props) => <Component {...props} />} />;
};

export default NonPrivateRoute;
