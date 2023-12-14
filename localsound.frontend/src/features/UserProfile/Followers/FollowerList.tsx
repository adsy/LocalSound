import { Icon } from "semantic-ui-react";
import { UserSummaryModel } from "../../../app/model/dto/user-summary.model";
import InfoBanner from "../../../common/banner/InfoBanner";
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
        <InfoBanner className="fade-in mb-2 mx-3">
          <div className="d-flex flex-row justify-content-center align-items-center">
            <Icon
              name="user"
              size="small"
              className="follower-icon d-flex align-items-center justify-content-center"
            />
            <div className="ml-2">
              <p className="mb-0">
                {isFollowers
                  ? "This account currently has no followers, be the first one to follow them."
                  : "This account is currently not following any other artists."}
              </p>
            </div>
          </div>
        </InfoBanner>
      )}
    </div>
  );
};

export default FollowingList;
