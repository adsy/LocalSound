import { UserModel } from "../../../app/model/dto/user.model";
import { useLayoutEffect, useState } from "react";
import { UserSummaryModel } from "./../../../app/model/dto/user-summary.model";
import agent from "../../../api/agent";
import { ProfileTabs } from "../../../app/model/enums/ProfileTabTypes";
import FollowerList from "./FollowerList";
import ErrorBanner from "./../../../common/banner/ErrorBanner";
import useFixMissingScroll from "../../../common/hooks/UseLoadMoreWithoutScroll";

interface Props {
  profileDetails: UserModel;
  currentTab: ProfileTabs;
  viewingOwnProfile: boolean;
}

const Following = ({
  profileDetails,
  currentTab,
  viewingOwnProfile,
}: Props) => {
  const [followers, setFollowers] = useState<UserSummaryModel[]>([]);
  const [loadError, setLoadError] = useState<string | null>();
  const [page, setPage] = useState(0);
  const [loading, setLoading] = useState(false);
  const [canLoadMore, setCanLoadMore] = useState(true);

  const getMoreFollowing = async () => {
    if (currentTab === ProfileTabs.Following && canLoadMore) {
      try {
        setLoading(true);

        var result = await agent.Account.getProfileFollowing(
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
    fetchMoreItems: getMoreFollowing,
  });

  useLayoutEffect(() => {
    setLoadError(null);
    (async () => {
      if (currentTab === ProfileTabs.Following && canLoadMore) {
        try {
          setLoading(true);

          var result = await agent.Account.getProfileFollowing(
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
    })();
  }, [currentTab]);

  return (
    <>
      {loadError ? (
        <ErrorBanner children={loadError} />
      ) : (
        <FollowerList
          followers={followers}
          isFollowers={false}
          canLoadMore={canLoadMore}
          getMore={getMoreFollowing}
          loading={loading}
          viewingOwnProfile={viewingOwnProfile}
        />
      )}
    </>
  );
};

export default Following;
