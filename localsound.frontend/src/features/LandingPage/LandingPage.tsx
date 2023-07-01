import { useDispatch, useSelector } from "react-redux";
import LandingPageBanner from "./LandingPageBanner";
import { useLayoutEffect } from "react";
import { State } from "../../app/model/redux/state";
import { useHistory } from "react-router-dom";

const LandingPage = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();
  const history = useHistory();
  console.log("landing-page");

  useLayoutEffect(() => {
    if (userDetails) {
      history.push("/home");
    }
  }, [userDetails]);

  return (
    <div id="landing-page">
      <LandingPageBanner />
      {/* <LandingPageFunctionality /> */}
    </div>
  );
};

export default LandingPage;
