import { Icon } from "semantic-ui-react";
import { UserSummaryModel } from "../../../app/model/dto/user-summary.model";
import InfoBanner from "../../../common/banner/InfoBanner";
import FollowerSquare from "../../../common/components/Follower/FollowerSquare";
import InfiniteScroll from "react-infinite-scroll-component";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";

interface Props {
  followers: UserSummaryModel[];
  isFollowers: boolean;
  getMore: () => void;
  canLoadMore: boolean;
  loading: boolean;
}

const FollowingList = ({
  followers,
  isFollowers,
  getMore,
  canLoadMore,
  loading,
}: Props) => {
  return (
    <div id="follower-list">
      {loading ? (
        <div className="h-100 d-flex justify-content-center align-self-center">
          <InPageLoadingComponent height={80} width={80} />
        </div>
      ) : followers?.length > 0 ? (
        <InfiniteScroll
          dataLength={followers.length} //This is important field to render the next data
          next={() => getMore()}
          hasMore={canLoadMore}
          loader={<></>}
        >
          <div className="d-flex flex-row flex-wrap w-100 m-auto">
            {followers.map((follower, index) => (
              <div key={index} className="col-3 col-md-2">
                <FollowerSquare key={index} follower={follower} />
              </div>
            ))}
          </div>
        </InfiniteScroll>
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
