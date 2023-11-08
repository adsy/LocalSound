import FollowerSquare from "../../../common/components/Follower/FollowerSquare";
import { UserModel } from "../../../app/model/dto/user.model";

interface Props {
  artistDetails: UserModel;
}

const Followers = ({ artistDetails }: Props) => {
  return (
    <div className="d-flex flex-column mt-5">
      <div className="d-flex flex-column">
        <h4 className="section-title">Followers</h4>
        {/* <div className="d-flex flex-row flex-wrap">
          {mockData().map((count, index) => (
            <div className="col-2">
              <FollowerSquare key={index} follower={followers[0]} />
            </div>
          ))}
        </div> */}
      </div>
      <div className="d-flex flex-column mt-5">
        <h4 className="section-title">Following</h4>
        {/* <div className="d-flex flex-row flex-wrap">
          {mockData().map((count, index) => (
            <FollowerSquare key={index} follower={followers[0]} />
          ))}
        </div> */}
      </div>
    </div>
  );
};

export default Followers;
