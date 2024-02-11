import { useState } from "react";
import { Tab, Tabs } from "react-bootstrap";
import { UserModel } from "../../../app/model/dto/user.model";
import Followers from "../Followers/Followers";
import { ArtistProfileTabs } from "../../../app/model/enums/artistProfileTabTypes";
import Following from "../Followers/Following";
import NonArtistDetails from "./NonArtistDetails";
import ProfileBanner from "../ProfileBanner";

interface Props {
  loggedInUser: UserModel;
  artistDetails: UserModel;
  viewingOwnProfile: boolean;
}

const NonArtistProfile = ({
  loggedInUser,
  artistDetails,
  viewingOwnProfile,
}: Props) => {
  const [currentTab, setCurrentTab] = useState(
    ArtistProfileTabs.ProfileDetails
  );
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
            setPhotoUpdateError={setPhotoUpdateError}
          />
          <div className="component-container">
            <Tabs
              id="controlled-tab-example"
              activeKey={currentTab}
              onSelect={(k) => {
                switch (k) {
                  case "0": {
                    setCurrentTab(ArtistProfileTabs.ProfileDetails);
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
                eventKey={ArtistProfileTabs.ProfileDetails}
                title="PROFILE DETAILS"
                className=""
              >
                <NonArtistDetails
                  userDetails={artistDetails}
                  photoUpdateError={photoUpdateError}
                  setCurrentTab={setCurrentTab}
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
                  viewingOwnProfile={viewingOwnProfile}
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
