import { useDispatch, useSelector } from "react-redux";
import LandingPageBanner from "./LandingPageBanner";
import { useLayoutEffect } from "react";
import { State } from "../../app/model/redux/state";
import { useHistory } from "react-router-dom";
import Login from "../../features/Authentication/Login/Login";
import Register from "../../features/Authentication/Register/Register";
import { Button } from "react-bootstrap";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import ConfirmEmailPopUp from "../Authentication/ConfirmEmail/ConfirmEmailPopUp";

const LandingPage = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const history = useHistory();
  const dispatch = useDispatch();

  useLayoutEffect(() => {
    if (userDetails && userDetails.emailConfirmed) {
      history.push("/home");
    }
    if (userDetails && !userDetails.emailConfirmed) {
      dispatch(
        handleToggleModal({
          open: true,
          body: <ConfirmEmailPopUp />,
          size: "mini",
        })
      );
    }
  }, [userDetails]);

  const handleAuthenticationRequest = (isLogin: boolean) => {
    if (isLogin) {
      dispatch(
        handleToggleModal({
          open: true,
          body: <Login />,
          size: "tiny",
        })
      );
    } else {
      dispatch(
        handleToggleModal({
          open: true,
          body: <Register />,
          size: "tiny",
        })
      );
    }
  };

  return (
    <div id="landing-page" className="fade-in">
      <LandingPageBanner />
      <div className="d-flex flex-row justify-content-center">
        <Button
          className="black-button mr-2 auth-button"
          onClick={() => handleAuthenticationRequest(true)}
        >
          <h4>Login</h4>
        </Button>
        <Button
          className="black-button auth-button"
          onClick={() => handleAuthenticationRequest(false)}
        >
          <h4>Create account</h4>
        </Button>
      </div>
      <div className="p-2 text-align-justify">
        <p className="mt-4">
          Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed at
          scelerisque sapien. Mauris vehicula felis dolor. Curabitur orci justo,
          dapibus eget dui ac, efficitur tincidunt lorem. Suspendisse in felis
          eget turpis fringilla faucibus quis quis elit. Vestibulum et aliquet
          metus, vitae auctor enim. Sed convallis turpis elit. Sed gravida neque
          sit amet diam facilisis, eu tristique est placerat. Maecenas bibendum
          vel massa ac rutrum. Proin facilisis mauris et tellus elementum
          viverra id ut urna. Donec ut feugiat ex. Morbi pellentesque, est ut
          pulvinar tempus, massa justo consectetur risus, non lacinia tellus mi
          et velit. Vivamus ut ipsum sapien. Maecenas congue velit lorem, sed
          tempus ipsum faucibus nec.
        </p>
        <p className="mt-4">
          Integer ac egestas elit. Nullam aliquam, ante ut varius vulputate,
          mauris urna sodales ligula, non aliquam orci tortor vel eros. Aliquam
          bibendum, ante at convallis consectetur, sem purus aliquam orci, vitae
          faucibus risus velit eget mauris. Nullam lobortis magna ut rutrum
          porta. Vestibulum aliquet massa ut leo fermentum commodo. Nam sit amet
          quam varius, varius ante vitae, sollicitudin lacus. Ut scelerisque,
          felis eu dignissim pretium, lacus erat posuere est, et commodo mauris
          eros nec tortor. Fusce aliquam, felis pharetra sodales ullamcorper,
          tellus ex semper ipsum, in molestie nisi nulla elementum arcu.
          Interdum et malesuada fames ac ante ipsum primis in faucibus.
        </p>
      </div>
      <div className="landing-page-container"></div>
      {/* <LandingPageFunctionality /> */}
    </div>
  );
};

export default LandingPage;
