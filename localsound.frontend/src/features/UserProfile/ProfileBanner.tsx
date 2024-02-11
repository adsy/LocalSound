import { useLayoutEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { handleAppLoading } from "../../app/redux/actions/applicationSlice";
import { UserModel } from "../../app/model/dto/user.model";
import { AccountImageModel } from "../../app/model/dto/account-image.model";
import { ProfileTabs } from "../../app/model/enums/ProfileTabTypes";
import { AccountImageTypes } from "../../app/model/enums/accountImageTypes";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import EditArtist from "./Artist/Edit/EditArtist";
import UploadTrackForm from "./Artist/Uploads/UploadTrackForm";
import { ArtistTrackUploadModel } from "../../app/model/dto/artist-track-upload.model";
import AddArtistPackage from "./Artist/ArtistPackages/AddArtistPackage";
import { ArtistPackageModel } from "../../app/model/dto/artist-package.model";
import { handleUpdateProfileFollowCount } from "../../app/redux/actions/pageDataSlice";
import signalHub from "../../api/signalR";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import agent from "../../api/agent";
import Login from "../Authentication/Login/Login";
import { Icon } from "semantic-ui-react";
import { Button } from "react-bootstrap";
import ErrorBanner from "../../common/banner/ErrorBanner";
import InPageLoadingComponent from "../../app/layout/InPageLoadingComponent";
import EditCoverPhoto from "./Artist/Edit/EditCoverPhoto";
import ProfileSummary from "./ProfileSummary";

interface Props {
  loggedInUser: UserModel;
  profileDetails: UserModel;
  viewingOwnProfile: boolean;
  setCurrentTab: (tab: ProfileTabs) => void;
  tracks?: ArtistTrackUploadModel[];
  setTracks?: (tracks: ArtistTrackUploadModel[]) => void;
  setPackages?: (packages: ArtistPackageModel[]) => void;
  setPhotoUpdateError: (error: string) => void;
}

const ProfileBanner = ({
  loggedInUser,
  profileDetails,
  viewingOwnProfile,
  setCurrentTab,
  tracks,
  setTracks,
  setPackages,
  setPhotoUpdateError,
}: Props) => {
  const [coverImage, setCoverImage] = useState<AccountImageModel | null>(null);
  const [updatingCoverPhoto, setUpdatingCoverPhoto] = useState(false);
  const [updateFollowerError, setUpdateFollowerError] = useState<string | null>(
    null
  );
  const [file, setFile] = useState<File | null>(null);
  const [imgsLoaded, setImgsLoaded] = useState(false);
  const [isFollowing, setIsFollowing] = useState(false);
  const [updatingFollowingStatus, setUpdatingFollowingStatus] = useState(false);
  const dispatch = useDispatch();

  const editArtistProfile = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: <EditArtist userDetails={profileDetails} />,
        size: "large",
      })
    );
  };

  const uploadTrack = () => {
    if (tracks && setTracks) {
      dispatch(
        handleToggleModal({
          open: true,
          body: (
            <UploadTrackForm
              userDetails={profileDetails}
              tracks={tracks}
              setTracks={setTracks}
            />
          ),
          size: "large",
        })
      );
      setCurrentTab(ProfileTabs.Uploads);
    }
  };

  const createPackage = () => {
    if (setPackages) {
      dispatch(
        handleToggleModal({
          open: true,
          body: (
            <AddArtistPackage
              userDetails={profileDetails}
              setPackages={setPackages}
            />
          ),
          size: "large",
        })
      );
      setCurrentTab(ProfileTabs.Packages);
    }
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

  useLayoutEffect(() => {
    if (profileDetails.isFollowing) {
      setIsFollowing(true);
    }
  }, [profileDetails.memberId, profileDetails?.isFollowing]);

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
    setCurrentTab(ProfileTabs.ProfileDetails);
    if (profileDetails?.images?.length > 0) {
      const IMAGES = [...profileDetails.images];
      Promise.all(IMAGES.map((image) => loadImage(image)))
        .then(() => {
          const coverImage = profileDetails?.images?.find(
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
  }, [profileDetails.memberId, profileDetails.images]);

  const updateArtistFollow = async (follow: boolean) => {
    if (loggedInUser) {
      if (!updatingFollowingStatus) {
        setUpdatingFollowingStatus(true);
        setUpdateFollowerError(null);
        try {
          if (follow) {
            await agent.Account.followArtist(
              loggedInUser.memberId,
              profileDetails.memberId
            );
            setIsFollowing(true);

            dispatch(
              handleUpdateProfileFollowCount(profileDetails.followerCount + 1)
            );
            signalHub.createNotification({
              receiverMemberId: profileDetails.memberId,
              message: `${
                loggedInUser.customerType === CustomerTypes.NonArtist
                  ? `${loggedInUser.firstName} ${loggedInUser.lastName} has started following you. Check out their profile!`
                  : `${loggedInUser.name} has started following you.`
              }`,
              redirectUrl: `/${loggedInUser.profileUrl}`,
            });
          } else {
            await agent.Account.unfollowArtist(
              loggedInUser.memberId,
              profileDetails.memberId
            );
            setIsFollowing(false);
            dispatch(
              handleUpdateProfileFollowCount(profileDetails.followerCount - 1)
            );
          }
        } catch (err: any) {
          console.log(err);
          setUpdateFollowerError(err);
        }
        setUpdatingFollowingStatus(false);
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
  };

  return (
    <>
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
                              <Icon name="photo" className="m-0 photo-icon" />
                            </h4>
                          </label>
                          <input
                            type="file"
                            id="exampleInput"
                            style={{ display: "none" }}
                            onChange={(event) => {
                              if (event && event.target && event.target.files) {
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
                        {tracks && setTracks ? (
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
                        ) : null}
                        {profileDetails.canAddPackage && setPackages ? (
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
                        className="btn white-button edit-profile-button w-fit-content d-flex flex-row follow-btn"
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
                <ProfileSummary
                  userDetails={profileDetails}
                  isArtist={
                    profileDetails.customerType === CustomerTypes.Artist
                  }
                />
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
                <ProfileSummary
                  userDetails={profileDetails}
                  isArtist={
                    profileDetails.customerType === CustomerTypes.Artist
                  }
                />
              </div>
            </div>
          )}
        </div>
      ) : (
        <InPageLoadingComponent withContainer={true} height={100} width={100} />
      )}
    </>
  );
};

export default ProfileBanner;
