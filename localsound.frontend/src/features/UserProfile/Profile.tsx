import { useDispatch, useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import ArtistProfile from "./Artist/ArtistProfile";
import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { UserModel } from "../../app/model/dto/user.model";
import { useHistory } from "react-router-dom";
import agent from "../../api/agent";
import ProfileNotFound from "./ProfileNotFound";
import { handleAppLoading } from "../../app/redux/actions/applicationSlice";

const UserProfileSummary = () => {
  const dispatch = useDispatch();
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const [profile, setProfile] = useState<UserModel | null>(null);
  const [noMatch, setNoMatch] = useState(false);
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
    setProfile(null);
    const getProfile = async () => {
      dispatch(handleAppLoading(true));
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

      dispatch(handleAppLoading(false));
    };

    getProfile().catch((err) => {
      setNoMatch(true);
    });
  }, [userDetail, history.location]);

  useEffect(() => {
    if (userDetail && viewingOwnProfile) setProfile(userDetail);
  }, [userDetail]);

  return (
    <>
      <div id="user-profile">
        {!noMatch &&
        profile &&
        profile?.customerType === CustomerTypes.Artist ? (
          <ArtistProfile
            loggedInUser={userDetail!}
            artistDetails={profile}
            setProfile={setProfile}
            viewingOwnProfile={viewingOwnProfile}
          />
        ) : null}
        {noMatch && !profile ? <ProfileNotFound /> : null}
      </div>
    </>
  );
};

export default UserProfileSummary;
