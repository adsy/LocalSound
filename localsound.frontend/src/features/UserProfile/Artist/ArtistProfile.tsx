import { useLayoutEffect, useState } from "react";
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
import ArtistSummary from "./ArtistSummary";
import { Icon } from "semantic-ui-react";
import UploadTrackForm from "./Uploads/UploadTrackForm";
import UploadList from "./Uploads/UploadList";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import agent from "../../../api/agent";
import Followers from "../Followers/Followers";
import { ArtistProfileTabs } from "../../../app/model/enums/artistProfileTabTypes";
import Following from "../Followers/Following";
import ErrorBanner from "../../../common/banner/ErrorBanner";

interface Props {
  loggedInUser: UserModel;
  artistDetails: UserModel;
  viewingOwnProfile: boolean;
  setProfile: (artistDetails: UserModel) => void;
}

const ArtistProfile = ({
  loggedInUser,
  artistDetails,
  viewingOwnProfile,
  setProfile,
}: Props) => {
  const [updatingCoverPhoto, setUpdatingCoverPhoto] = useState(false);
  const [submittingRequest, setSubmittingRequest] = useState(false);
  const [updateFollowerError, setUpdateFollowerError] = useState<string | null>(
    null
  );
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>(null);
  const [file, setFile] = useState<File | null>(null);
  const [imgsLoaded, setImgsLoaded] = useState(false);
  const [coverImage, setCoverImage] = useState<AccountImageModel | null>(null);
  const [currentTab, setCurrentTab] = useState(ArtistProfileTabs.ArtistDetails);
  const [tracks, setTracks] = useState<ArtistTrackUploadModel[]>([]);
  const [isFollowing, setIsFollowing] = useState(false);
  const [canLoadMore, setCanLoadMore] = useState(true);
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

  useLayoutEffect(() => {
    if (artistDetails.isFollowing) {
      setIsFollowing(true);
    }
  }, [artistDetails.memberId, artistDetails?.isFollowing]);

  useLayoutEffect(() => {
    if (loggedInUser?.memberId === artistDetails.memberId) {
      const IMAGES = [...loggedInUser.images];

      Promise.all(IMAGES.map((image) => loadImage(image)))
        .then(() => {
          const coverImage = loggedInUser?.images?.find(
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
    } else {
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
    }
  }, [artistDetails.memberId, loggedInUser?.images]);

  const editArtistProfile = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: <EditArtist userDetails={artistDetails} />,
        size: "large",
      })
    );
  };

  const uploadTrack = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <UploadTrackForm
            userDetails={artistDetails}
            tracks={tracks}
            setTracks={setTracks}
          />
        ),
        size: "large",
      })
    );
    setCurrentTab(ArtistProfileTabs.Uploads);
  };

  const updateArtistFollow = async (follow: boolean) => {
    setUpdateFollowerError(null);
    try {
      if (follow) {
        await agent.Artist.followArtist(
          loggedInUser.memberId,
          artistDetails.memberId
        );
        setIsFollowing(true);
        var localArtist = artistDetails;
        localArtist.followerCount += 1;
        setProfile(localArtist);
      } else {
        await agent.Artist.unfollowArtist(
          loggedInUser.memberId,
          artistDetails.memberId
        );
        setIsFollowing(false);
        var localArtist = artistDetails;
        localArtist.followerCount -= 1;
        setProfile(localArtist);
      }
    } catch (err: any) {
      setUpdateFollowerError(err);
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
    <>
      {imgsLoaded && !submittingRequest ? (
        <div id="artist-profile">
          <div className="d-flex flex-column flex-wrap p-0 fade-in w-100">
            <div className="d-flex flex-row banner-holder">
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
                                  <Icon
                                    name="photo"
                                    className="m-0 photo-icon"
                                  />
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
                        <div className="w-100 d-flex flex-row justify-content-between">
                          <div>
                            {updateFollowerError ? (
                              <ErrorBanner
                                children={updateFollowerError}
                                className="w-100 mb-0"
                              />
                            ) : null}
                          </div>
                          <a
                            onClick={async () => {
                              if (isFollowing) {
                                await updateArtistFollow(false);
                              } else {
                                await updateArtistFollow(true);
                              }
                            }}
                            target="_blank"
                            className="btn black-button edit-profile-button w-fit-content d-flex flex-row"
                          >
                            <h4>
                              {isFollowing ? (
                                <span className="mr-1 fade-in">
                                  Unfollow artist
                                </span>
                              ) : (
                                <span className="mr-1 fade-in">
                                  Follow artist
                                </span>
                              )}
                              <Icon
                                name="heart"
                                className="mt-0 mb-0 mr-0 ml-1"
                              />
                            </h4>
                          </a>
                        </div>
                      )}
                    </div>
                    <ArtistSummary userDetails={artistDetails} />
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
                    <ArtistSummary userDetails={artistDetails} />
                  </div>
                </div>
              )}
            </div>
            <div className="p-3 component-container">
              <Tabs
                id="controlled-tab-example"
                activeKey={currentTab}
                onSelect={(k) => {
                  switch (k) {
                    case "0": {
                      setCurrentTab(ArtistProfileTabs.ArtistDetails);
                      break;
                    }
                    case "1": {
                      setCurrentTab(ArtistProfileTabs.Uploads);
                      break;
                    }
                    case "2": {
                      setCurrentTab(ArtistProfileTabs.Followers);
                      break;
                    }
                    case "3": {
                      setCurrentTab(ArtistProfileTabs.Following);
                      break;
                    }
                    case "4": {
                      setCurrentTab(ArtistProfileTabs.Packages);
                      break;
                    }
                  }
                }}
                className="mb-4"
              >
                <Tab
                  eventKey={ArtistProfileTabs.ArtistDetails}
                  title="ARTIST DETAILS"
                  className=""
                >
                  <ArtistDetails
                    userDetails={artistDetails}
                    photoUpdateError={photoUpdateError}
                    setCurrentTab={setCurrentTab}
                  />
                </Tab>
                <Tab
                  eventKey={ArtistProfileTabs.Uploads}
                  title="UPLOADS"
                  className=""
                >
                  <UploadList
                    userDetails={artistDetails}
                    currentTab={currentTab}
                    tracks={tracks}
                    setTracks={setTracks}
                    viewingOwnProfile={viewingOwnProfile}
                    canLoadMore={canLoadMore}
                    setCanLoadMore={setCanLoadMore}
                  />
                </Tab>
                <Tab
                  eventKey={ArtistProfileTabs.Followers}
                  title="FOLLOWERS"
                  className=""
                >
                  <Followers
                    artistDetails={artistDetails}
                    currentTab={currentTab}
                  />
                </Tab>
                <Tab
                  eventKey={ArtistProfileTabs.Following}
                  title="FOLLOWING"
                  className=""
                >
                  <Following
                    artistDetails={artistDetails}
                    currentTab={currentTab}
                  />
                </Tab>
                <Tab
                  eventKey={ArtistProfileTabs.Packages}
                  title="BOOKING"
                  className=""
                >
                  <></>
                </Tab>
              </Tabs>
            </div>
          </div>
        </div>
      ) : !imgsLoaded && !submittingRequest ? (
        <div className="d-flex justify-content-center align-self-center">
          <InPageLoadingComponent
            height={100}
            width={100}
            withContainer={true}
          />
        </div>
      ) : submittingRequest ? (
        <div className="d-flex justify-content-center align-self-center">
          <InPageLoadingComponent
            height={100}
            width={100}
            withContainer={true}
          />
        </div>
      ) : null}
    </>
  );
};

export default ArtistProfile;
