import { useDispatch, useSelector } from "react-redux";
import LandingPageBanner from "./LandingPageBanner";
import LandingPageFunctionality from "./LandingPageFunctionality";
import { useEffect } from "react";
import { State } from "../../app/model/redux/state";
import { useHistory } from "react-router-dom";

const LandingPage = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();
  const history = useHistory();

  useEffect(() => {
    if (userDetails) {
      history.push("/home");
    }
  }, []);

  return (
    <div id="landing-page">
      <LandingPageBanner />
      {/* <LandingPageFunctionality /> */}
    </div>
  );
};

export default LandingPage;
