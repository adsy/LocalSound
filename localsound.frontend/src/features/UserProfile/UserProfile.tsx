import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import ArtistProfile from "./Artist/ArtistProfile";
import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { UserModel } from "../../app/model/dto/user.model";
import { useHistory } from "react-router-dom";
import agent from "../../api/agent";

const UserProfileSummary = () => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const [profile, setProfile] = useState<UserModel | null>(null);
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
      if (!userDetail) {
        var result = await agent.Profile.getProfile(profileUrl);

        if (result) {
          setProfile(result);
        } else {
          // TODO: Do something with error here
        }
      } else {
        setProfile(userDetail);
      }
    };

    getProfile().catch((err) => {
      console.log(err);
      //TODO: Do something with the error here
    });

    //TODO: If it fails, redirect to a cannot find user component
  }, [userDetail]);

  return (
    <div id="user-profile">
      {profile?.customerType === CustomerTypes.Artist ? (
        <ArtistProfile userDetails={profile} />
      ) : null}
    </div>
  );
};

export default UserProfileSummary;
