import { Col } from "react-bootstrap";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { UserModel } from "../../../app/model/dto/user.model";
import Label from "../../../common/components/Label/Label";
import { Image } from "semantic-ui-react";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import userImg from "../../../assets/placeholder.png";
import { ArtistProfileTabs } from "../../../app/model/enums/artistProfileTabTypes";

interface Props {
  photoUpdateError: string | null;
  userDetails: UserModel;
  setCurrentTab: (tab: ArtistProfileTabs) => void;
}

const ArtistDetails = ({
  photoUpdateError,
  userDetails,
  setCurrentTab,
}: Props) => {
  const userPhoto = userDetails.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
  );

  return (
    <div className="d-flex flex-row flex-wrap">
      <Col md={12} lg={4} className="p-0 left-col">
        <div className="d-flex flex-column p-2 py-0">
          {photoUpdateError ? (
            <div className="d-flex flex-row">
              <ErrorBanner className="w-100">{photoUpdateError}</ErrorBanner>
            </div>
          ) : null}
          <Image
            src={userPhoto ? userPhoto.accountImageUrl : userImg}
            size="small"
            circular
            className={`profile-photo mb-4 summary-photo align-self-center`}
          />
          <div className="d-flex flex-column pb-4">
            <div className="d-flex flex-row">
              <div className="d-flex flex-column col-6">
                <div
                  className="about-text m-0 px-3 left-col text-right"
                  onClick={() => {
                    setCurrentTab(ArtistProfileTabs.Followers);
                  }}
                >
                  <a className="follow-text">
                    {userDetails.followerCount} follower
                    {userDetails.followerCount !== 1 ? "s" : ""}
                  </a>
                </div>
              </div>
              <div className="d-flex flex-column col-6">
                <div
                  className="about-text m-0 px-3 text-left "
                  onClick={() => {
                    setCurrentTab(ArtistProfileTabs.Following);
                  }}
                >
                  <a className="follow-text">
                    {userDetails.followingCount} following
                  </a>
                </div>
              </div>
            </div>
          </div>
          <div className="d-flex flex-column pb-4 genre-mobile">
            <div className="d-flex flex-row flex-wrap justify-content-center">
              <h4 className="section-title inverse">
                <span className="purple-highlight">Genres</span>
              </h4>
            </div>
            <div className="about-text d-flex flex-row flex-wrap justify-content-center">
              {userDetails.genres.map((genre, index) => (
                <span key={index}>
                  <Label label={genre.genreName} id={genre.genreId} />
                </span>
              ))}
            </div>
          </div>
          <div className="d-flex flex-column pb-4">
            <div className="d-flex flex-row flex-wrap justify-content-center">
              <h4 className="section-title mb-3 mt-4">
                <span className="purple-highlight">Event Types</span>
              </h4>
            </div>
            <div className="about-text d-flex flex-row flex-wrap justify-content-center">
              {userDetails.eventTypes.map((eventType, index) => (
                <span key={index}>
                  <Label
                    label={eventType.eventTypeName}
                    id={eventType.eventTypeId}
                  />
                </span>
              ))}
            </div>
          </div>
          <div className="d-flex flex-column pb-4">
            <div className="d-flex flex-row flex-wrap justify-content-center">
              <h4 className="section-title mb-3 mt-4">
                <span className="purple-highlight">Equipment</span>
              </h4>
            </div>
            <div className="about-text d-flex flex-row flex-wrap justify-content-center">
              {userDetails.equipment.map((equipment, index) => (
                <span key={index}>
                  <Label
                    label={equipment.equipmentName}
                    id={equipment.equipmentId}
                  />
                </span>
              ))}
            </div>
          </div>
        </div>
      </Col>
      <Col md={12} lg={8} className="p-0 right-col">
        <div className="p-2 py-0">
          <div className="d-flex flex-column pb-4">
            <div className="d-flex">
              <h4 className="section-title inverse">
                <span className="purple-highlight">About</span>
              </h4>
            </div>
            <span className="about-text">{userDetails.aboutSection}</span>
            <div className=" d-flex flex-row flex-wrap mt-5">
              {userDetails?.soundcloudUrl ? (
                <>
                  <a
                    href={userDetails.soundcloudUrl}
                    target="_blank"
                    className="btn soundcloud-button w-fit-content d-flex flex-row mb-1 mr-1"
                  >
                    <div className="soundcloud-icon align-self-center"></div>
                  </a>
                </>
              ) : null}
              {userDetails?.spotifyUrl ? (
                <>
                  <a
                    href={userDetails.spotifyUrl}
                    target="_blank"
                    className="btn spotify-button w-fit-content d-flex flex-row mb-1 mr-1"
                  >
                    <div className="spotify-icon align-self-center"></div>
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
                    <div className="soundcloud-icon align-self-center"></div>
                  </a>
                </>
              ) : null}
            </div>
          </div>
        </div>
      </Col>
    </div>
  );
};

export default ArtistDetails;
