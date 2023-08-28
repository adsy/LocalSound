import { useDispatch } from "react-redux";
import bg from "../../../assets/landing-page-banner/banner2.jpg";
import img from "../../../assets/icons/user.svg";
import { Button, Col, Row } from "react-bootstrap";
import { Image } from "semantic-ui-react";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import EditArtistProfile from "./EditArtistProfile";
import { UserModel } from "../../../app/model/dto/user.model";

interface Props {
  userDetails: UserModel;
  viewingOwnProfile: boolean;
}

const ArtistProfile = ({ userDetails, viewingOwnProfile }: Props) => {
  const dispatch = useDispatch();

  const bannerStyle = {
    backgroundImage: `url(${bg})`,
    backgroundSize: "cover",
    backgroundAttachment: "inherit",
    backgroundPosition: "center center",
    backgroundRepeat: "no-repeat",
    height: "24rem",
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
            <div className="details-container flex-wrap">
              <Image
                src={img}
                size="small"
                circular
                className="mb-2 profile-img"
              />
              <span className="user-name align-self-end mb-0 ml-1">
                {userDetails?.name}
              </span>

              <div className="ml-2 d-flex flex-row flex-wrap">
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
                {viewingOwnProfile ? (
                  <Button
                    onClick={() => editArtistProfile()}
                    className="black-button m-1 edit-profile-btn"
                  >
                    <h4>Edit profile</h4>
                  </Button>
                ) : null}
              </div>
            </div>
          </div>
          <div className="d-flex flex-column p-2">
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title">Followers</h4>
              <div className="d-flex flex-row">
                <span className="about-text m-0 pr-3">0 followers</span>
                <span className="about-text m-0">0 following</span>
              </div>
            </div>
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title">About</h4>
              <span className="about-text">{userDetails.about}</span>
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
