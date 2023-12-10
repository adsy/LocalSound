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
import { Placeholder } from "semantic-ui-react";

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
      <div className="component-container p-3">
        <div className="d-flex flex-row justify-content-center mt-2 mb-2">
          <Button
            className="white-button mr-4 auth-button"
            onClick={() => handleAuthenticationRequest(true)}
          >
            <h1>
              <span className="button-highlight">Login</span>
            </h1>
          </Button>
          <Button
            className="white-button  ml-4 auth-button"
            onClick={() => handleAuthenticationRequest(false)}
          >
            <h1>
              <span className="button-highlight">Register</span>
            </h1>
          </Button>
        </div>
        <div className="d-flex justify-content-center mb-2">
          <Button className="white-button auth-button">
            <h1>
              <span className="button-highlight">FIND AN ARTIST</span>
            </h1>
          </Button>
        </div>
      </div>
      <div className="text-align-justify component-container mb-5">
        <div>
          <div className="d-flex flex-row">
            <div className=" pr-5">
              <h4 className="search-text">GET YOUR PROFILE OUT THERE</h4>
              <p>
                Welcome to your artist's playground! Creating your profile is
                the first step to unleashing your musical magic. Customize it by
                choosing the genres that define your vibe and the types of gigs
                you own – from intimate acoustic sets to electrifying stadium
                performances. Showcase your sonic mastery by uploading tracks
                that will have fans hitting the repeat button. With a profile
                that's as unique as your sound, attracting followers is a
                breeze. Soon enough, you'll not only be setting the beats but
                also getting booked for gigs that match your musical journey.
                Let the profile building commence – it's time to amplify your
                presence in the music scene!
              </p>
            </div>
            <div>
              <Placeholder style={{ height: 150, width: 250 }}>
                <Placeholder.Image />
              </Placeholder>
            </div>
          </div>
        </div>
        <div className="mt-5">
          <div className="d-flex flex-row">
            <div>
              <Placeholder style={{ height: 150, width: 250 }}>
                <Placeholder.Image />
              </Placeholder>
            </div>
            <div className="pl-5">
              <h4 className="search-text text-right">
                SEARCH FOR AN ARTIST OR DJ FOR YOUR NEXT PARTY
              </h4>
              <p>
                Discover the heartbeat of your local music scene effortlessly on
                our platform. Whether you're into indie vibes or electronic
                beats, our search features allow you to find artists in your
                area or explore by genre. Dive into a sonic adventure, sample
                tracks, and when you find the artist that resonates with your
                party vibe, it's just a click away from booking them for your
                next event. Elevate your gatherings with the perfect soundtrack
                – your dream artist is just a search away
              </p>
            </div>
          </div>
        </div>
      </div>
      {/* <LandingPageFunctionality /> */}
    </div>
  );
};

export default LandingPage;
