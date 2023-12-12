import { UserSummaryModel } from "../../../app/model/dto/user-summary.model";
import FollowerSquare from "../../../common/components/Follower/FollowerSquare";

interface Props {
  followers: UserSummaryModel[];
  listRef: React.RefObject<HTMLDivElement>;
  isFollowers: boolean;
}

const FollowingList = ({ followers, listRef, isFollowers }: Props) => {
  return (
    <div id="follower-list" ref={listRef}>
      {followers?.length > 0 ? (
        <div className="d-flex flex-row flex-wrap w-100 m-auto">
          {followers.map((follower, index) => (
            <div key={index} className="col-3 col-md-2">
              <FollowerSquare key={index} follower={follower} />
            </div>
          ))}
        </div>
      ) : (
        <div className="d-flex flex-row justify-content-center">
          <div className="d-flex flex-column text-center align-items-center black-alert">
            <div className="ml-2">
              <p className="mb-0">
                {isFollowers
                  ? "This account currently has no followers, be the first one to follow them."
                  : "This account is currently not following any other artists."}
              </p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default FollowingList;
