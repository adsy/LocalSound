import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { CustomerTypes } from "../../app/model/enums/customerTypes";
import ArtistProfile from "./ArtistProfile";

const UserProfileSummary = () => {
  const userDetail = useSelector((state: State) => state.user.userDetails);

  return (
    <div id="user-profile">
      {userDetail?.customerType === CustomerTypes.Artist ? (
        <ArtistProfile />
      ) : null}
    </div>
  );
};

export default UserProfileSummary;
