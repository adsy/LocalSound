import { useEffect, useLayoutEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Button, Tab, Tabs } from "react-bootstrap";
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
import Login from "../../Authentication/Login/Login";
import AddArtistPackage from "./ArtistPackages/AddArtistPackage";
import ArtistPackages from "./ArtistPackages/ArtistPackages";
import { ArtistPackageModel } from "../../../app/model/dto/artist-package.model";
import { handleAppLoading } from "../../../app/redux/actions/applicationSlice";
import { handleUpdateProfileFollowCount } from "../../../app/redux/actions/profileSlice";
import signalHub from "../../../api/signalR";

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
  const [updateFollowerError, setUpdateFollowerError] = useState<string | null>(
    null
  );
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>(null);
  const [file, setFile] = useState<File | null>(null);
  const [imgsLoaded, setImgsLoaded] = useState(false);
  const [coverImage, setCoverImage] = useState<AccountImageModel | null>(null);
  const [currentTab, setCurrentTab] = useState(ArtistProfileTabs.ArtistDetails);
  const [tracks, setTracks] = useState<ArtistTrackUploadModel[]>([]);
  const [packages, setPackages] = useState<ArtistPackageModel[]>([]);
  const [isFollowing, setIsFollowing] = useState(false);
  const [updatingFollowingStatus, setUpdatingFollowingStatus] = useState(false);
  const dispatch = useDispatch();

  const loadImage = (image: AccountImageModel) => {
    if (imgsLoaded) {
      dispatch(handleAppLoading(true));
      setImgsLoaded(false);
    }
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
    setCurrentTab(ArtistProfileTabs.ArtistDetails);
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
          dispatch(handleAppLoading(false));
        });
    } else {
      setCoverImage(null);
      setImgsLoaded(true);
    }
  }, [artistDetails.memberId, artistDetails.images]);

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

  const createPackage = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <AddArtistPackage
            userDetails={artistDetails}
            setPackages={setPackages}
          />
        ),
        size: "large",
      })
    );
    setCurrentTab(ArtistProfileTabs.Packages);
  };

  const updateArtistFollow = async (follow: boolean) => {
    setUpdatingFollowingStatus(true);
    if (loggedInUser) {
      setUpdateFollowerError(null);
      try {
        if (follow) {
          await agent.Artist.followArtist(
            loggedInUser.memberId,
            artistDetails.memberId
          );
          setIsFollowing(true);
          dispatch(
            handleUpdateProfileFollowCount(artistDetails.followerCount + 1)
          );
          signalHub.createNotification({
            receiverMemberId: artistDetails.memberId,
            message: `You have a new follower!`,
            redirectUrl: "",
          });
        } else {
          await agent.Artist.unfollowArtist(
            loggedInUser.memberId,
            artistDetails.memberId
          );
          setIsFollowing(false);
          dispatch(
            handleUpdateProfileFollowCount(artistDetails.followerCount - 1)
          );
        }
      } catch (err: any) {
        console.log(err);
        setUpdateFollowerError(err);
      }
    } else {
      dispatch(
        handleToggleModal({
          open: true,
          body: <Login />,
          size: "tiny",
        })
      );
    }
    setUpdatingFollowingStatus(false);
  };

  const generateCoverImage = () => {
    if (coverImage?.accountImageUrl) {
      return {
        backgroundSize: "cover",
        backgroundAttachment: "inherit",
        backgroundPosition: "center center",
        backgroundRepeat: "no-repeat",
        height: "30rem",
        backgroundImage: `url(${coverImage?.accountImageUrl!})`,
      };
    }
    return {
      backgroundSize: "cover",
      backgroundAttachment: "inherit",
      backgroundPosition: "center center",
      backgroundRepeat: "no-repeat",
      height: "30rem",
      backgroundImage: "linear-gradient(20deg, #232323, #001514, #232323)",
    };
  };

  return (
    <>
      <div id="artist-profile">
        <div className="d-flex flex-column flex-wrap p-0 fade-in w-100">
          {imgsLoaded ? (
            <div className="d-flex flex-row banner-holder fade-in">
              {!updatingCoverPhoto && !file ? (
                <div
                  style={{ ...generateCoverImage() }}
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
                              title="edit-details"
                            >
                              <h4>
                                <Icon name="pencil" className="m-0" />
                              </h4>
                            </a>
                            <a
                              onClick={() => uploadTrack()}
                              target="_blank"
                              className="btn black-button edit-profile-button w-fit-content d-flex flex-row mb-3 mr-1"
                              title="upload-track"
                            >
                              <h4>
                                <Icon name="upload" className="m-0" />
                              </h4>
                            </a>
                            {artistDetails.canAddPackage ? (
                              <a
                                onClick={() => createPackage()}
                                target="_blank"
                                className="btn black-button edit-profile-button w-fit-content d-flex flex-row mb-3"
                                title="create-package"
                              >
                                <h4>
                                  <Icon name="box" className="m-0" />
                                </h4>
                              </a>
                            ) : null}
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
                          <Button
                            onClick={async () => {
                              if (isFollowing) {
                                await updateArtistFollow(false);
                              } else {
                                await updateArtistFollow(true);
                              }
                            }}
                            className="btn white-button edit-profile-button w-fit-content d-flex flex-row"
                            title="update-following"
                          >
                            {!updatingFollowingStatus ? (
                              <h4 className="fade-in">
                                {isFollowing ? (
                                  <span className="mr-1 fade-in ml-1">
                                    Unfollow artist
                                  </span>
                                ) : (
                                  <span className="mr-1 fade-in ml-1">
                                    Follow artist
                                  </span>
                                )}
                                <Icon
                                  name="heart"
                                  className="mt-0 mb-0 mr-0 ml-1"
                                />
                              </h4>
                            ) : (
                              <InPageLoadingComponent height={20} width={20} />
                            )}
                          </Button>
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
                    setPhotoUpdateError={setPhotoUpdateError}
                  />
                  <div className="details-container flex-wrap">
                    <ArtistSummary userDetails={artistDetails} />
                  </div>
                </div>
              )}
            </div>
          ) : (
            <InPageLoadingComponent
              withContainer={true}
              height={100}
              width={100}
            />
          )}
          <div className="component-container">
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
                title="PACKAGES"
                className=""
              >
                <ArtistPackages
                  artistDetails={artistDetails}
                  currentTab={currentTab}
                  viewingOwnProfile={viewingOwnProfile}
                  packages={packages}
                  setPackages={setPackages}
                />
              </Tab>
            </Tabs>
          </div>
        </div>
      </div>
    </>
  );
};

export default ArtistProfile;
