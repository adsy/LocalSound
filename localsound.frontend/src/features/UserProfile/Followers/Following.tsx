import { UserModel } from "../../../app/model/dto/user.model";
import { useLayoutEffect, useRef, useState } from "react";
import { UserSummaryModel } from "./../../../app/model/dto/user-summary.model";
import agent from "../../../api/agent";
import { debounce } from "lodash";
import { ArtistProfileTabs } from "../../../app/model/enums/artistProfileTabTypes";
import FollowerList from "./FollowerList";
import ErrorBanner from "./../../../common/banner/ErrorBanner";

interface Props {
  artistDetails: UserModel;
  currentTab: ArtistProfileTabs;
}

const Following = ({ artistDetails, currentTab }: Props) => {
  const [followers, setFollowers] = useState<UserSummaryModel[]>([]);
  const [loadError, setLoadError] = useState<string | null>();
  const [page, setPage] = useState(0);
  const listRef = useRef<HTMLDivElement>(null);
  const [loading, setLoading] = useState(false);
  const [canLoadMore, setCanLoadMore] = useState(true);

  window.onscroll = debounce(() => {
    if (listRef?.current) {
      if (
        currentTab === ArtistProfileTabs.Following &&
        !loading &&
        canLoadMore &&
        window.innerHeight +
          document.documentElement.scrollTop -
          listRef?.current?.offsetHeight <
          500
      ) {
        setPage(page + 1);
      }
    }
  }, 100);

  useLayoutEffect(() => {
    setLoadError(null);
    (async () => {
      if (currentTab === ArtistProfileTabs.Following && canLoadMore) {
        try {
          setLoading(true);

          var result = await agent.Profile.getProfileFollowing(
            artistDetails.memberId,
            page
          );

          setLoading(false);
          setFollowers([...followers, ...result.followers]);
          setCanLoadMore(result.canLoadMore);
        } catch (err: any) {
          setLoadError(err);
        }
      }
    })();
  }, [page, currentTab]);

  return (
    <>
      {loadError ? (
        <ErrorBanner children={loadError} />
      ) : (
        <FollowerList
          followers={followers}
          listRef={listRef}
          isFollowers={false}
        />
      )}
    </>
  );
};

export default Following;