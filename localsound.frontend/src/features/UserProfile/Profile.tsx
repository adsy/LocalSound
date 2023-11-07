import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import ArtistProfile from "./Artist/ArtistProfile";
import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { UserModel } from "../../app/model/dto/user.model";
import { useHistory } from "react-router-dom";
import agent from "../../api/agent";
import ProfileNotFound from "./ProfileNotFound";
import InPageLoadingComponent from "../../app/layout/InPageLoadingComponent";

const UserProfileSummary = () => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const [profile, setProfile] = useState<UserModel | null>(null);
  const [noMatch, setNoMatch] = useState(false);
  const [loading, setLoading] = useState(true);
  const [viewingOwnProfile, setViewingOwnProfile] = useState(false);
  const history = useHistory();
  const abortControllerRef = useRef<AbortController>(new AbortController());
  const controller = abortControllerRef.current;

  useEffect(() => {
    return () => {
      controller.abort();
    };
  }, [controller]);

  useLayoutEffect(() => {
    const getProfile = async () => {
      var profileUrl = history.location.pathname.slice(1);

      if (!userDetail || userDetail?.profileUrl !== profileUrl) {
        if (profileUrl?.length > 0) {
          var result = await agent.Profile.getProfile(profileUrl);
          setProfile(result);
          setViewingOwnProfile(false);
        }
      } else if (userDetail) {
        setProfile(userDetail);
        setViewingOwnProfile(true);
      }
    };

    getProfile()
      .catch((err) => {
        setNoMatch(true);
      })
      .finally(() => {
        setLoading(false);
      });
  }, [history.location]);

  return (
    <div id="user-profile">
      {loading ? (
        <div className="h-100 align-self-center d-flex justify-content-center">
          <InPageLoadingComponent
            height={150}
            width={150}
            content="Loading profile..."
          />
        </div>
      ) : null}
      {!loading && profile?.customerType === CustomerTypes.Artist ? (
        <ArtistProfile
          loggedInUser={userDetail!}
          artistDetails={profile}
          setProfile={setProfile}
          viewingOwnProfile={viewingOwnProfile}
        />
      ) : null}
      {noMatch ? <ProfileNotFound /> : null}
    </div>
  );
};

export default UserProfileSummary;
