import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import bg from "../../../assets/landing-page-banner/banner3.jpg";
import img from "../../../assets/icons/user.svg";
import { Button, Col, Row } from "react-bootstrap";
import { Image } from "semantic-ui-react";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import EditArtistProfile from "./EditArtistProfile";

const ArtistProfile = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const bannerStyle = {
    backgroundImage: `url(${bg})`,
    backgroundSize: "cover",
    backgroundAttachment: "inherit",
    backgroundPosition: "center center",
    backgroundRepeat: "no-repeat",
    height: "16rem",
    // borderRadius: "0px 0px 100px 0px",
    // border: "1px solid black",
    boxShadow: "rgba(0, 0, 0, 1) 0px -1px 4px 0px inset",
  };

  const editArtistProfile = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: <EditArtistProfile />,
        size: "large",
      })
    );
  };

  return (
    <>
      <div className="d-flex flex-col flex-wrap h-100 p-0">
        <Col md={12} lg={6} className="p-0 left-col">
          <div style={bannerStyle} className="profile-banner position-relative">
            <Button
              onClick={() => editArtistProfile()}
              className="black-button m-1 edit-profile-btn"
            >
              Edit profile
            </Button>
            <div className="details-container flex-wrap">
              <Image
                src={img}
                size="small"
                circular
                className="mt-2 profile-img"
              />
              <span className="user-name align-self-end mb-0 ml-1">
                {userDetails?.name}
              </span>
            </div>
          </div>
          <div className="d-flex flex-column p-2">
            <div className="d-flex flex-row flex-wrap">
              {userDetails?.soundcloudUrl ? (
                <>
                  <a
                    href={userDetails.soundcloudUrl}
                    target="_blank"
                    className="btn soundcloud-button w-fit-content d-flex flex-row mb-3 mr-1"
                  >
                    <div className="soundcloud-icon align-self-center mr-2"></div>
                    <h4 className="align-self-center mt-0">Soundcloud</h4>
                  </a>
                </>
              ) : null}
              {userDetails?.spotifyUrl ? (
                <>
                  <a
                    href={userDetails.spotifyUrl}
                    target="_blank"
                    className="btn spotify-button w-fit-content d-flex flex-row mb-3 mr-1"
                  >
                    <div className="soundcloud-icon align-self-center mr-2"></div>
                    <h4 className="align-self-center mt-0">Spotify</h4>
                  </a>
                </>
              ) : null}
              {userDetails?.youtubeUrl ? (
                <>
                  <a
                    href={userDetails.youtubeUrl}
                    target="_blank"
                    className="btn youtube-button w-fit-content d-flex flex-row mb-3"
                  >
                    <div className="soundcloud-icon align-self-center mr-2"></div>
                    <h4 className="align-self-center mt-0">Youtube</h4>
                  </a>
                </>
              ) : null}
            </div>
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title pb-2">About</h4>
              <span className="about-text">
                Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do
                eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut
                enim ad minim veniam, quis nostrud exercitation ullamco laboris
                nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor
                in reprehenderit in voluptate velit esse cillum dolore eu fugiat
                nulla pariatur. Excepteur sint occaecat cupidatat non proident,
                sunt in culpa qui officia deserunt mollit anim id est laborum.
              </span>
            </div>
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title">Followers</h4>
              <div className="d-flex flex-row pt-2">
                <h5 className="m-0 pr-3">0 followers</h5>
                <h5 className="m-0">0 following</h5>
              </div>
            </div>
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title">Upcoming gigs</h4>
            </div>
          </div>
        </Col>
        <Col md={12} lg={6} className="p-0 right-col">
          <div className="d-flex flex-column p-2">
            <h4 className="section-title">Uploaded tracks</h4>
            <span>track</span>
            <span>track</span>
            <span>track</span>
            <span>track</span>
            <span>track</span>
            <span>track</span>
            <span>track</span>
          </div>
        </Col>
      </div>
    </>
  );
};

export default ArtistProfile;
