import { useDispatch } from "react-redux";
import { Col } from "react-bootstrap";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import { UserModel } from "../../../app/model/dto/user.model";
import EditArtist from "./Edit/EditArtist";
import GenreTypeLabel from "../../../common/components/Label/GenreTypeLabel";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import { useEffect, useState } from "react";

interface Props {
  userDetails: UserModel;
  viewingOwnProfile: boolean;
}

const ArtistProfile = ({ userDetails, viewingOwnProfile }: Props) => {
  const [banner, setBanner] = useState({});
  const dispatch = useDispatch();
  const profileImage = userDetails.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
  );
  const coverImage = userDetails.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.CoverImage
  );

  useEffect(() => {
    const bannerStyle = {
      backgroundImage: `url(${coverImage?.accountImageUrl!})`,
      backgroundSize: "cover",
      backgroundAttachment: "inherit",
      backgroundPosition: "center center",
      backgroundRepeat: "no-repeat",
      height: "24rem",
      // borderRadius: "0px 0px 100px 0px",
      // border: "1px solid black",
      boxShadow: "rgba(0, 0, 0, 1) 0px -1px 4px 0px inset",
    };
    setBanner(bannerStyle);
  }, [userDetails]);

  const editArtistProfile = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: <EditArtist userDetails={userDetails} />,
        size: "large",
      })
    );
  };

  return (
    <>
      <div className="d-flex flex-col flex-wrap p-0 fade-in">
        <Col md={12} lg={6} className="p-0 left-col">
          <div style={banner} className="profile-banner position-relative">
            <div className="details-container flex-wrap">
              <div className="d-flex justify-content-end pt-1 pr-1 w-100">
                {viewingOwnProfile ? (
                  <>
                    <a
                      onClick={() => editArtistProfile()}
                      target="_blank"
                      className="btn black-button edit-profile-button w-fit-content d-flex flex-row mb-3"
                    >
                      <h4>Edit profile</h4>
                    </a>
                  </>
                ) : null}
              </div>
              <div className="d-flex flex-column">
                <div className="d-flex flex-row ml-2">
                  <div className="d-flex flex-column">
                    <span className="user-name mb-0">{userDetails?.name}</span>
                    <div className="d-flex flex-column pb-2 genre-desktop w-75">
                      <div className="about-text">
                        {userDetails.genres.map((genre, index) => (
                          <span key={index}>
                            <GenreTypeLabel genre={genre} />
                          </span>
                        ))}
                      </div>
                    </div>
                  </div>
                </div>
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
                        <div className="spotify-icon align-self-center mr-2"></div>
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
              </div>
            </div>
          </div>
          <div className="d-flex flex-column p-2">
            <div className="d-flex flex-column pb-2 genre-mobile">
              <h4 className="section-title">Genres</h4>
              <div className="about-text">
                {userDetails.genres.map((genre, index) => (
                  <span key={index}>
                    <GenreTypeLabel genre={genre} />
                  </span>
                ))}
              </div>
            </div>
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title">Followers</h4>
              <div className="d-flex flex-row">
                <span className="about-text m-0 pr-3">0 followers</span>
                <span className="about-text m-0">0 following</span>
              </div>
            </div>
            <div className="d-flex flex-column pb-4">
              <h4 className="section-title">About</h4>
              <span className="about-text">{userDetails.aboutSection}</span>
            </div>
            {/* <div className="d-flex flex-column pb-4">
              <h4 className="section-title">Upcoming gigs</h4>
            </div> */}
          </div>
        </Col>
        <Col md={12} lg={6} className="p-0 right-col">
          <div className="d-flex flex-column p-2">
            <h4 className="section-title">Uploads</h4>
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
