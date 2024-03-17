import { UserModel } from "../../../app/model/dto/user.model";
import { useLayoutEffect, useState } from "react";
import { UserSummaryModel } from "./../../../app/model/dto/user-summary.model";
import agent from "../../../api/agent";
import { ProfileTabs } from "../../../app/model/enums/ProfileTabTypes";
import FollowerList from "./FollowerList";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import useFixMissingScroll from "../../../common/hooks/UseLoadMoreWithoutScroll";

interface Props {
  profileDetails: UserModel;
  viewingOwnProfile: boolean;
}

const Followers = ({ profileDetails, viewingOwnProfile }: Props) => {
  const [followers, setFollowers] = useState<UserSummaryModel[]>([]);
  const [page, setPage] = useState(0);
  const [loading, setLoading] = useState(true);
  const [canLoadMore, setCanLoadMore] = useState(true);
  const [loadError, setLoadError] = useState<string | null>();

  const getMoreFollowers = async () => {
    if (canLoadMore) {
      try {
        setLoading(true);

        var result = await agent.Account.getProfileFollowers(
          profileDetails.memberId,
          page
        );

        setFollowers([...followers, ...result.followers]);
        setCanLoadMore(result.canLoadMore);
      } catch (err: any) {
        setLoadError(err);
      }
      setLoading(false);
      setPage(page + 1);
    }
  };

  useFixMissingScroll({
    hasMoreItems: canLoadMore,
    fetchMoreItems: getMoreFollowers,
  });

  return (
    <>
      {loadError ? (
        <ErrorBanner children={loadError} />
      ) : (
        <FollowerList
          followers={followers}
          isFollowers={true}
          getMore={getMoreFollowers}
          canLoadMore={canLoadMore}
          loading={loading}
          viewingOwnProfile={viewingOwnProfile}
        />
      )}
    </>
  );
};

export default Followers;
