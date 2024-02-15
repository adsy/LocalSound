import { useDispatch, useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import ArtistProfile from "./Artist/ArtistProfile";
import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { useHistory } from "react-router-dom";
import agent from "../../api/agent";
import ProfileNotFound from "./ProfileNotFound";
import InPageLoadingComponent from "../../app/layout/InPageLoadingComponent";
import { handleSetProfile } from "../../app/redux/actions/pageDataSlice";
import NonArtistProfile from "./NonArtist/NonArtistProfile";

const UserProfileSummary = () => {
  const dispatch = useDispatch();
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const profile = useSelector((state: State) => state.pageData.profileData);
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

  const getProfile = async () => {
    var profileUrl = history.location.pathname.slice(1);

    if (!loading && profileUrl !== profile?.profileUrl) {
      dispatch(handleSetProfile(null));
      setLoading(true);
      if (userDetail?.profileUrl !== profileUrl) {
        if (profileUrl?.length > 0) {
          var result = await agent.Account.getProfile(profileUrl);
          dispatch(handleSetProfile(result));
          setViewingOwnProfile(false);
        }
      } else if (userDetail) {
        dispatch(handleSetProfile(userDetail));
        setViewingOwnProfile(true);
      }
    }
  };

  useLayoutEffect(() => {
    (async () => {
      await getProfile()
        .catch((err) => {
          setNoMatch(true);
        })
        .finally(() => {
          setLoading(false);
        });
    })();
  }, [userDetail, history.location]);

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
            profileDetails={profile}
            viewingOwnProfile={viewingOwnProfile}
          />
        </div>
      ) : null}
      {!noMatch &&
      !loading &&
      profile?.customerType === CustomerTypes.NonArtist ? (
        <div id="user-profile">
          <NonArtistProfile
            loggedInUser={userDetail!}
            profileDetails={profile}
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
