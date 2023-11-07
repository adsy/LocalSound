import { Image } from "semantic-ui-react";
import { UserSummaryModel } from "../../../app/model/dto/user-summary.model";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import { useHistory } from "react-router-dom";
import { useLayoutEffect, useState } from "react";
import userImg from "../../../assets/icons/user.svg";

interface Props {
  follower: UserSummaryModel;
}

const FollowerCircle = ({ follower }: Props) => {
  const history = useHistory();
  const [profileImage, setProfileImage] = useState<string>();

  useLayoutEffect(() => {
    var profileImg = follower.images.find(
      (x) => x.accountImageTypeId === AccountImageTypes.ProfileImage
    );
    setProfileImage(profileImg?.accountImageUrl);
  }, []);

  const goProfile = () => {
    history.push(`/${follower.profileUrl}`);
  };

  return (
    <div id="follower-image" onClick={() => goProfile()}>
      <Image
        circular
        size={"mini"}
        src={profileImage ? profileImage : userImg}
      />
    </div>
  );
};

export default FollowerCircle;
