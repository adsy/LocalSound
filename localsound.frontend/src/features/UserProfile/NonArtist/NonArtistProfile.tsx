import { useState } from "react";
import { Tab, Tabs } from "react-bootstrap";
import { UserModel } from "../../../app/model/dto/user.model";
import { ProfileTabs } from "../../../app/model/enums/ProfileTabTypes";
import Following from "../Followers/Following";
import ProfileBanner from "../ProfileBanner";
import Favourites from "../Favourites/Favourites";
import NonArtistDetails from "./NonArtistDetails";

interface Props {
  loggedInUser: UserModel;
  profileDetails: UserModel;
  viewingOwnProfile: boolean;
}

const NonArtistProfile = ({
  loggedInUser,
  profileDetails,
  viewingOwnProfile,
}: Props) => {
  const [currentTab, setCurrentTab] = useState(ProfileTabs.ProfileDetails);
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>(null);

  return (
    <>
      <div id="artist-profile">
        <div className="d-flex flex-column flex-wrap p-0 fade-in w-100">
          <ProfileBanner
            loggedInUser={loggedInUser}
            profileDetails={profileDetails}
            viewingOwnProfile={viewingOwnProfile}
            setCurrentTab={setCurrentTab}
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
                  }
                }
              }}
              className="mb-4"
            >
              <Tab
                eventKey={ProfileTabs.ProfileDetails}
                title="PROFILE DETAILS"
                className=""
              >
                <NonArtistDetails
                  userDetails={profileDetails}
                  photoUpdateError={photoUpdateError}
                  setCurrentTab={setCurrentTab}
                />
              </Tab>
              <Tab
                eventKey={ProfileTabs.Following}
                title="FOLLOWING"
                className=""
              >
                <Following
                  profileDetails={profileDetails}
                  currentTab={currentTab}
                  viewingOwnProfile={viewingOwnProfile}
                />
              </Tab>
              <Tab
                eventKey={ProfileTabs.LikedSongs}
                title="FAVOURITES"
                className=""
              >
                <Favourites
                  currentTab={currentTab}
                  profileDetails={profileDetails}
                  viewingOwnProfile={viewingOwnProfile}
                />
              </Tab>
            </Tabs>
          </div>
        </div>
      </div>
    </>
  );
};

export default NonArtistProfile;
