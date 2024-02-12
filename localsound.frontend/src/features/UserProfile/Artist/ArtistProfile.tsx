import { useState } from "react";
import { Tab, Tabs } from "react-bootstrap";
import { UserModel } from "../../../app/model/dto/user.model";
import ArtistDetails from "./ArtistDetails";
import UploadList from "./Uploads/UploadList";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import Followers from "../Followers/Followers";
import { ProfileTabs } from "../../../app/model/enums/ProfileTabTypes";
import Following from "../Followers/Following";
import ArtistPackages from "./ArtistPackages/ArtistPackages";
import { ArtistPackageModel } from "../../../app/model/dto/artist-package.model";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import ProfileBanner from "../ProfileBanner";

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
  const [currentTab, setCurrentTab] = useState(ProfileTabs.ProfileDetails);
  const [tracks, setTracks] = useState<ArtistTrackUploadModel[]>([]);
  const [packages, setPackages] = useState<ArtistPackageModel[]>([]);
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>(null);

  return (
    <>
      <div id="artist-profile">
        <div className="d-flex flex-column flex-wrap p-0 fade-in w-100">
          <ProfileBanner
            loggedInUser={loggedInUser}
            profileDetails={artistDetails}
            viewingOwnProfile={viewingOwnProfile}
            setCurrentTab={setCurrentTab}
            tracks={tracks}
            setTracks={setTracks}
            setPackages={setPackages}
            setPhotoUpdateError={setPhotoUpdateError}
          />
          <div className="component-container">
            <Tabs
              id="controlled-tab-example"
              activeKey={currentTab}
              onSelect={(k) => {
                switch (k) {
                  case "0": {
                    setCurrentTab(ProfileTabs.ProfileDetails);
                    break;
                  }
                  case "1": {
                    setCurrentTab(ProfileTabs.Uploads);
                    break;
                  }
                  case "2": {
                    setCurrentTab(ProfileTabs.Followers);
                    break;
                  }
                  case "3": {
                    setCurrentTab(ProfileTabs.Following);
                    break;
                  }
                  case "4": {
                    setCurrentTab(ProfileTabs.Packages);
                    break;
                  }
                  case "5": {
                    setCurrentTab(ProfileTabs.LikedSongs);
                    break;
                  }
                }
              }}
              className="mb-4"
            >
              <Tab
                eventKey={ProfileTabs.ProfileDetails}
                title="ARTIST DETAILS"
                className=""
              >
                <ArtistDetails
                  userDetails={artistDetails}
                  photoUpdateError={photoUpdateError}
                  setCurrentTab={setCurrentTab}
                />
              </Tab>
              <Tab eventKey={ProfileTabs.Uploads} title="UPLOADS" className="">
                <UploadList
                  userDetails={artistDetails}
                  currentTab={currentTab}
                  tracks={tracks}
                  setTracks={setTracks}
                  viewingOwnProfile={viewingOwnProfile}
                />
              </Tab>
              <Tab
                eventKey={ProfileTabs.LikedSongs}
                title="FAVOURITES"
                className=""
              >
                <div>need to add a liked songs component</div>
              </Tab>
              <Tab
                eventKey={ProfileTabs.Followers}
                title="FOLLOWERS"
                className=""
              >
                <Followers
                  artistDetails={artistDetails}
                  currentTab={currentTab}
                  viewingOwnProfile={viewingOwnProfile}
                />
              </Tab>
              <Tab
                eventKey={ProfileTabs.Following}
                title="FOLLOWING"
                className=""
              >
                <Following
                  profileDetails={artistDetails}
                  currentTab={currentTab}
                  viewingOwnProfile={viewingOwnProfile}
                />
              </Tab>
              {viewingOwnProfile ||
              (!viewingOwnProfile &&
                loggedInUser?.customerType == CustomerTypes.NonArtist) ? (
                <Tab
                  eventKey={ProfileTabs.Packages}
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
              ) : null}
            </Tabs>
          </div>
        </div>
      </div>
    </>
  );
};

export default ArtistProfile;
