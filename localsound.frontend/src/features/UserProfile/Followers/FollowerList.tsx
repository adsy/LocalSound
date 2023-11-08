import { UserSummaryModel } from "../../../app/model/dto/user-summary.model";
import FollowerSquare from "../../../common/components/Follower/FollowerSquare";

interface Props {
  followers: UserSummaryModel[];
  listRef: React.RefObject<HTMLDivElement>;
  isFollowers: boolean;
}

const FollowingList = ({ followers, listRef, isFollowers }: Props) => {
  return (
    <div id="follower-list" ref={listRef} className="d-flex flex-column mt-2">
      {followers?.length > 0 ? (
        <div className="d-flex flex-row flex-wrap w-100">
          {followers.map((follower, index) => (
            <div key={index} className="col-4 col-md-2">
              <FollowerSquare key={index} follower={follower} />
            </div>
          ))}
        </div>
      ) : (
        <div className="d-flex flex-row justify-content-center justify-content-center mt-5">
          <h5>
            {isFollowers
              ? "This account currently has no followers, be the first one to follow them."
              : "This account is currently not following any other artists."}
          </h5>
        </div>
      )}
    </div>
  );
};

export default FollowingList;
