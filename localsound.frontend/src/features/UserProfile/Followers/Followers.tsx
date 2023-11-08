import FollowerSquare from "../../../common/components/Follower/FollowerSquare";
import { UserModel } from "../../../app/model/dto/user.model";
import { useLayoutEffect, useRef, useState } from "react";
import { UserSummaryModel } from "./../../../app/model/dto/user-summary.model";
import agent from "../../../api/agent";
import { debounce } from "lodash";

interface Props {
  artistDetails: UserModel;
  onFollowers: boolean;
}

const Followers = ({ artistDetails, onFollowers }: Props) => {
  const [followers, setFollowers] = useState<UserSummaryModel[]>([]);
  const [page, setPage] = useState(0);
  const listRef = useRef<HTMLDivElement>(null);
  const [loading, setLoading] = useState(false);
  const [canLoadMore, setCanLoadMore] = useState(true);

  window.onscroll = debounce(() => {
    if (listRef?.current) {
      if (
        onFollowers &&
        !loading &&
        canLoadMore &&
        window.innerHeight +
          document.documentElement.scrollTop -
          listRef?.current?.offsetHeight <
          1000
      ) {
        setPage(page + 1);
      }
    }
  }, 100);

  useLayoutEffect(() => {
    (async () => {
      if (onFollowers && canLoadMore) {
        try {
          setLoading(true);
          var result = await agent.Profile.getProfileFollowers(
            artistDetails.memberId,
            page
          );

          setLoading(false);
          setFollowers([...followers, ...result.followers]);
          setCanLoadMore(result.canLoadMore);
        } catch (err) {
          //TODO: handle error
        }
      }
    })();
  }, [page, onFollowers]);

  return (
    <div ref={listRef} className="d-flex flex-column mt-2">
      <div className="d-flex flex-row flex-wrap">
        {followers.map((follower, index) => (
          <div className="col-2">
            <FollowerSquare key={index} follower={follower} />
          </div>
        ))}
      </div>
    </div>
  );
};

export default Followers;
