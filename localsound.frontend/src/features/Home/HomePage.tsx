import { useDispatch, useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import InfoBanner from "../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import ArtistOnboarding from "../Onboarding/Artist/ArtistOnboarding";
import NonArtistOnboarding from "../Onboarding/NonArtist/NonArtistOnboarding";

const HomePage = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const openOnboardingModal = () => {
    if (userDetails?.customerType === CustomerTypes.Artist) {
      dispatch(
        handleToggleModal({
          body: <ArtistOnboarding />,
          size: "large",
          open: true,
        })
      );
    } else {
      dispatch(
        handleToggleModal({
          body: <NonArtistOnboarding />,
          size: "large",
          open: true,
        })
      );
    }
  };

  return (
    <div id="home-page" className="fade-in">
      {!userDetails?.accountSetupCompleted ? (
        <InfoBanner className="fade-in mb-2 mx-3 mt-3">
          <div className="d-flex flex-row align-items-center justify-content-center">
            <Icon
              name="user"
              size="small"
              className="follower-icon d-flex align-items-center justify-content-center"
            />
            <div className="ml-2">
              <p className="mb-0">
                <span className="ml-1 align-items-center info-banner-text">
                  It looks like you havent setup your account yet. Click{" "}
                  <span
                    className="link-button"
                    onClick={() => openOnboardingModal()}
                  >
                    here
                  </span>{" "}
                  to get started!
                </span>
              </p>
            </div>
          </div>
        </InfoBanner>
      ) : null}
      {/* <UserProfileSummary /> */}
    </div>
  );
};

export default HomePage;
