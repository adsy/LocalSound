import { Image } from "semantic-ui-react";
import { UserSummaryModel } from "../../../app/model/dto/user-summary.model";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import { useHistory } from "react-router-dom";
import { useLayoutEffect, useState } from "react";
import userImg from "../../../assets/placeholder.png";

interface Props {
  follower: UserSummaryModel;
}

const FollowerSquare = ({ follower }: Props) => {
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
    <div
      id="follower-square"
      className="d-flex flex-column align-items-center p-2"
      onClick={() => goProfile()}
    >
      <Image
        src={profileImage ? profileImage : userImg}
        circular
        // size="small"
        className="align-self-center"
      />
      <p className="section-title text-center">{follower.name}</p>
    </div>
  );
};

export default FollowerSquare;
