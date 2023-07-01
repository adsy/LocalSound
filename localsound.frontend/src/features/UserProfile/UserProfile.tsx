import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import ArtistProfile from "./Artist/ArtistProfile";
import { useLayoutEffect } from "react";

const UserProfileSummary = () => {
  const userDetail = useSelector((state: State) => state.user.userDetails);

  useLayoutEffect(() => {
    if (!userDetail) {
      //TODO: Send api request to get profile data
    }

    //TODO: If it fails, redirect to a cannot find user component
  }, [userDetail]);

  return (
    <div id="user-profile">
      {userDetail?.customerType === CustomerTypes.Artist ? (
        <ArtistProfile />
      ) : null}
    </div>
  );
};

export default UserProfileSummary;
