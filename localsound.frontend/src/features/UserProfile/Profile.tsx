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
  const [viewingOwnProfile, setViewingOwnProfile] = useState(false);
  const [loading, setLoading] = useState(false);
  const history = useHistory();
  const abortControllerRef = useRef<AbortController>(new AbortController());
  const controller = abortControllerRef.current;

  useEffect(() => {
    return () => {
      controller.abort();
    };
  }, [controller]);

  useLayoutEffect(() => {
    setProfile(null);
    setLoading(true);
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
  }, [userDetail, history.location]);

  useEffect(() => {
    if (userDetail && viewingOwnProfile) setProfile(userDetail);
  }, [userDetail]);

  return (
    <>
      {loading ? (
        <div className="d-flex align-items-center justify-content-center h-100 w-100 top-71">
          <InPageLoadingComponent
            height={100}
            width={100}
            withContainer={true}
          />
        </div>
      ) : null}
      {!noMatch &&
      !loading &&
      profile?.customerType === CustomerTypes.Artist ? (
        <div id="user-profile">
          <ArtistProfile
            loggedInUser={userDetail!}
            artistDetails={profile}
            setProfile={setProfile}
            viewingOwnProfile={viewingOwnProfile}
          />
        </div>
      ) : null}
      {noMatch && !loading && !profile ? (
        <div id="user-profile">
          <ProfileNotFound />
        </div>
      ) : null}
    </>
  );
};

export default UserProfileSummary;
