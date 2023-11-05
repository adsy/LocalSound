import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { Tab, Tabs } from "react-bootstrap";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import { UserModel } from "../../../app/model/dto/user.model";
import { AccountImageModel } from "../../../app/model/dto/account-image.model";
import EditArtist from "./Edit/EditArtist";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import EditCoverPhoto from "./Edit/EditCoverPhoto";
import ArtistDetails from "./ArtistDetails";
import ArtistBannerSummary from "./ArtistBannerSummary";
import { Icon } from "semantic-ui-react";
import ArtistUploadTrackForm from "./ArtistUploads/ArtistUploadTrackForm";
import ArtistUploadsList from "./ArtistUploads/ArtistUploadsList";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import agent from "../../../api/agent";

interface Props {
  loggedInUser: UserModel;
  artistDetails: UserModel;
  viewingOwnProfile: boolean;
}

const ArtistProfile = ({
  loggedInUser,
  artistDetails,
  viewingOwnProfile,
}: Props) => {
  const [updatingCoverPhoto, setUpdatingCoverPhoto] = useState(false);
  const [submittingRequest, setSubmittingRequest] = useState(false);
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>(null);
  const [file, setFile] = useState<File | null>(null);
  const [imgsLoaded, setImgsLoaded] = useState(false);
  const [coverImage, setCoverImage] = useState<AccountImageModel | null>(null);
  const [key, setKey] = useState<string | null>("artistDetails");
  const [onUploads, setOnUploads] = useState(false);
  const [uploading, setUploading] = useState(false);
  const [tracks, setTracks] = useState<ArtistTrackUploadModel[]>([]);
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
    if (artistDetails?.images?.length > 0) {
      const IMAGES = [...artistDetails.images];
      Promise.all(IMAGES.map((image) => loadImage(image)))
        .then(() => {
          const coverImage = artistDetails?.images?.find(
            (x) => x.accountImageTypeId == AccountImageTypes.CoverImage
          );

          if (coverImage) {
            setCoverImage(coverImage);
          }
        })
        .catch((err) => console.log("Failed to load images", err))
        .finally(() => {
          setImgsLoaded(true);
        });
    }
  }, [artistDetails.memberId]);

  const editArtistProfile = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <EditArtist
            userDetails={artistDetails}
            setSubmittingRequest={setSubmittingRequest}
          />
        ),
        size: "large",
      })
    );
  };

  const uploadTrack = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <ArtistUploadTrackForm
            userDetails={artistDetails}
            tracks={tracks}
            setTracks={setTracks}
          />
        ),
        size: "large",
      })
    );
    setKey("uploads");
    setUploading(true);
  };

  const followArtist = async () => {
    try {
      var result = await agent.Artist.followArtist(
        loggedInUser.memberId,
        artistDetails.memberId
      );

      //TODO: Update followers
    } catch (err) {
      //TODO: do something with error
    }
  };

  const bannerStyle = {
    backgroundSize: "cover",
    backgroundAttachment: "inherit",
    backgroundPosition: "center center",
    backgroundRepeat: "no-repeat",
    height: "30rem",
    boxShadow: "rgba(0, 0, 0, 1) 0px -1px 4px 0px inset",
  };

  return (
    <div id="artist-profile">
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
                              <h4>
                                <Icon name="photo" className="m-0 photo-icon" />
                              </h4>
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
                        <div className="d-flex flex-row">
                          <a
                            onClick={() => editArtistProfile()}
                            target="_blank"
                            className="btn black-button edit-profile-button w-fit-content d-flex flex-row mb-3 mr-1"
                          >
                            <h4>
                              <Icon name="pencil" className="m-0" />
                            </h4>
                          </a>
                          <a
                            onClick={() => uploadTrack()}
                            target="_blank"
                            className="btn black-button edit-profile-button w-fit-content d-flex flex-row mb-3"
                          >
                            <h4>
                              <Icon name="upload" className="m-0" />
                            </h4>
                          </a>
                        </div>
                      </>
                    ) : (
                      <div className="w-100 d-flex flex-row justify-content-end">
                        <a
                          onClick={() => followArtist()}
                          target="_blank"
                          className="btn black-button edit-profile-button w-fit-content d-flex flex-row"
                        >
                          <h4>
                            <span className="mr-1">Follow artist </span>
                            <Icon
                              name="heart"
                              className="mt-0 mb-0 mr-0 ml-1"
                            />
                          </h4>
                        </a>
                      </div>
                    )}
                  </div>
                  <ArtistBannerSummary userDetails={artistDetails} />
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
                  <ArtistBannerSummary userDetails={artistDetails} />
                </div>
              </div>
            )}
          </div>
          <div className="p-3">
            <Tabs
              id="controlled-tab-example"
              activeKey={key!}
              onSelect={(k) => {
                if (uploading) {
                  setUploading(!uploading);
                }
                setKey(k);
                if (k === "uploads") {
                  setOnUploads(true);
                } else {
                  setOnUploads(false);
                }
              }}
              className="mb-4"
            >
              <Tab eventKey="artistDetails" title="Artist details" className="">
                <ArtistDetails
                  userDetails={artistDetails}
                  photoUpdateError={photoUpdateError}
                />
              </Tab>
              <Tab eventKey="uploads" title="Uploads" className="">
                <ArtistUploadsList
                  userDetails={artistDetails}
                  onUploads={onUploads}
                  tracks={tracks}
                  setTracks={setTracks}
                  viewingOwnProfile={viewingOwnProfile}
                />
              </Tab>
            </Tabs>
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
    </div>
  );
};

export default ArtistProfile;
