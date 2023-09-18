import { useDispatch } from "react-redux";
import { Col, Row } from "react-bootstrap";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import { UserModel } from "../../../app/model/dto/user.model";
import EditArtist from "./Edit/EditArtist";
import Label from "../../../common/components/Label/Label";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import { useEffect, useState } from "react";
import { AccountImageModel } from "../../../app/model/dto/account-image.model";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import EditCoverPhoto from "./Edit/EditCoverPhoto";
import ErrorBanner from "../../../common/banner/ErrorBanner";

interface Props {
  userDetails: UserModel;
  viewingOwnProfile: boolean;
}

const ArtistProfile = ({ userDetails, viewingOwnProfile }: Props) => {
  const [updatingCoverPhoto, setUpdatingCoverPhoto] = useState(false);
  const [submittingRequest, setSubmittingRequest] = useState(false);
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>(null);
  const [file, setFile] = useState<File | null>(null);
  const [imgsLoaded, setImgsLoaded] = useState(true);
  const [coverImage, setCoverImage] = useState<AccountImageModel | null>(null);
  const dispatch = useDispatch();

  const loadImage = (image: AccountImageModel) => {
    if (imgsLoaded) setImgsLoaded(false);
    return new Promise((resolve, reject) => {
      const loadImg = new Image();
      loadImg.src = image.accountImageUrl;
      loadImg.onload = () => resolve(image.accountImageUrl);
      loadImg.onerror = (err) => reject(err);
    });
  };

  useEffect(() => {
    if (userDetails?.images?.length > 0) {
      const IMAGES = [...userDetails.images];
      Promise.all(IMAGES.map((image) => loadImage(image)))
        .then(() => {
          setImgsLoaded(true);

          const coverImage = userDetails?.images?.find(
            (x) => x.accountImageTypeId == AccountImageTypes.CoverImage
          );

          if (coverImage) {
            setCoverImage(coverImage);
          }
        })
        .catch((err) => console.log("Failed to load images", err));
    }
  }, [userDetails]);

  const editArtistProfile = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <EditArtist
            userDetails={userDetails}
            setSubmittingRequest={setSubmittingRequest}
          />
        ),
        size: "large",
      })
    );
  };

  const bannerStyle = {
    backgroundSize: "cover",
    backgroundAttachment: "inherit",
    backgroundPosition: "center center",
    backgroundRepeat: "no-repeat",
    height: "24rem",
    boxShadow: "rgba(0, 0, 0, 1) 0px -1px 4px 0px inset",
  };

  return (
    <>
      {imgsLoaded && !submittingRequest ? (
        <div className="d-flex flex-column flex-wrap p-0 fade-in w-100">
          <div className="d-flex flex-row">
            {!updatingCoverPhoto && !file ? (
              <div
                style={{
                  ...bannerStyle,
                  backgroundImage: `url(${coverImage?.accountImageUrl!})`,
                }}
                className="profile-banner position-relative w-100"
              >
                <div className="details-container flex-wrap">
                  <div className="d-flex flex-row justify-content-between pt-1 pr-1 pl-1 w-100">
                    {viewingOwnProfile ? (
                      <>
                        <div className="">
                          <div className="fade-in edit-cover-button">
                            <label
                              htmlFor="exampleInput"
                              className="btn black-button fade-in-out"
                            >
                              <h4>Update cover photo</h4>
                            </label>
                            <input
                              type="file"
                              id="exampleInput"
                              style={{ display: "none" }}
                              onChange={(event) => {
                                if (
                                  event &&
                                  event.target &&
                                  event.target.files
                                ) {
                                  setFile(event.target.files[0]);
                                  setUpdatingCoverPhoto(true);
                                }
                              }}
                            />
                          </div>
                        </div>
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
                        <span className="user-name mb-0">
                          {userDetails?.name}
                        </span>
                        <div className="d-flex flex-column pb-2 genre-desktop">
                          <div className="about-text">
                            {userDetails.genres.map((genre, index) => (
                              <span key={index}>
                                <Label
                                  label={genre.genreName}
                                  id={genre.genreId}
                                />
                              </span>
                            ))}
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ) : (
              <div className="position-relative cropping w-100">
                <EditCoverPhoto
                  file={file!}
                  setFile={setFile}
                  setUpdatingCoverPhoto={setUpdatingCoverPhoto}
                  setSubmittingRequest={setSubmittingRequest}
                  setPhotoUpdateError={setPhotoUpdateError}
                />
                <div className="details-container flex-wrap">
                  <div className=""></div>
                  <div className="d-flex flex-column">
                    <div className="d-flex flex-row ml-2">
                      <div className="d-flex flex-column">
                        <span className="user-name mb-0">
                          {userDetails?.name}
                        </span>
                        <div className="d-flex flex-column pb-2 genre-desktop">
                          <div className="about-text">
                            {userDetails.genres.map((genre, index) => (
                              <span key={index}>
                                <Label
                                  label={genre.genreName}
                                  id={genre.genreId}
                                />
                              </span>
                            ))}
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            )}
          </div>
          <div className="d-flex flex-row flex-wrap">
            <Col md={12} lg={6} className="p-0 left-col">
              <div className="d-flex flex-column p-2">
                {photoUpdateError ? (
                  <div className="d-flex flex-row">
                    <ErrorBanner className="w-100">
                      {photoUpdateError}
                    </ErrorBanner>
                  </div>
                ) : null}
                <div className=" d-flex flex-row flex-wrap">
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
                <div className="d-flex flex-column pb-2 genre-mobile">
                  <h4 className="section-title inverse">Genres</h4>
                  <div className="about-text">
                    {userDetails.genres.map((genre, index) => (
                      <span key={index}>
                        <Label label={genre.genreName} id={genre.genreId} />
                      </span>
                    ))}
                  </div>
                </div>
                <div className="d-flex flex-column pb-4">
                  <h4 className="section-title inverse">Event types</h4>
                  <div className="about-text">
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
                  <h4 className="section-title inverse">Equipment</h4>
                  <div className="about-text">
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

                <div className="d-flex flex-column pb-4">
                  <h4 className="section-title inverse">Followers</h4>
                  <div className="d-flex flex-row">
                    <span className="about-text m-0 pr-3">0 followers</span>
                    <span className="about-text m-0">0 following</span>
                  </div>
                </div>

                <div className="d-flex flex-column pb-4">
                  <h4 className="section-title inverse">About</h4>
                  <span className="about-text">{userDetails.aboutSection}</span>
                </div>
                {/* <div className="d-flex flex-column pb-4">
              <h4 className="section-title inverse">Upcoming gigs</h4>
            </div> */}
              </div>
            </Col>
            <Col md={12} lg={6} className="p-0 right-col">
              <div className="d-flex flex-column p-2">
                <h4 className="section-title inverse">Uploads</h4>
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
        </div>
      ) : !imgsLoaded && !submittingRequest ? (
        <div className="h-100 d-flex justify-content-center align-self-center">
          <InPageLoadingComponent
            height={150}
            width={150}
            content="Loading data..."
          />
        </div>
      ) : submittingRequest ? (
        <div className="h-100 d-flex justify-content-center align-self-center">
          <InPageLoadingComponent
            height={150}
            width={150}
            content="Submitting data..."
          />
        </div>
      ) : null}
    </>
  );
};

export default ArtistProfile;
