import { useEffect, useLayoutEffect, useState } from "react";
import { UserModel } from "../../../app/model/dto/user.model";
import { ProfileTabs } from "../../../app/model/enums/ProfileTabTypes";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Image } from "semantic-ui-react";
import wavePulse from "../../../assets/wave-pulse-1-svgrepo-com.svg";
import InfiniteScroll from "react-infinite-scroll-component";
import agent from "../../../api/agent";
import useFixMissingScroll from "../../../common/hooks/UseLoadMoreWithoutScroll";
import Track from "../../../common/components/Track/Track";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { handleSetTrackList } from "../../../app/redux/actions/playerSlice";
import { PlaylistTypes } from "../../../app/model/enums/playlistTypes";

interface Props {
  currentTab: ProfileTabs;
  profileDetails: UserModel;
  viewingOwnProfile: boolean;
}

const Favourites = ({
  currentTab,
  profileDetails,
  viewingOwnProfile,
}: Props) => {
  const [loading, setLoading] = useState(true);
  const [favourites, setFavourites] = useState<ArtistTrackUploadModel[]>([]);
  const [canLoadMore, setCanLoadMore] = useState(true);
  const [favouritesError, setFavouritesError] = useState<string | null>(null);
  const playerState = useSelector((state: State) => state.player);
  const dispatch = useDispatch();

  const getMoreFavourites = async () => {
    if (canLoadMore && currentTab === ProfileTabs.LikedSongs) {
      try {
        setLoading(true);

        var songLikeId = favourites[favourites.length - 1]?.songLikeId;

        var result = await agent.Tracks.getTracks(
          profileDetails!.memberId,
          PlaylistTypes.Favourites,
          songLikeId
        );

        setFavourites([...favourites, ...result.trackList]);

        setCanLoadMore(result.canLoadMore);
      } catch (err: any) {
        setFavouritesError(err);
      }
      setLoading(false);
    }
  };

  useFixMissingScroll({
    hasMoreItems: canLoadMore,
    fetchMoreItems: () => getMoreFavourites(),
  });

  useLayoutEffect(() => {
    (async () => {
      if (currentTab === ProfileTabs.LikedSongs) {
        await getMoreFavourites();
      }
    })();

    return () => {
      setFavourites([]);
      setCanLoadMore(true);
    };
  }, [currentTab]);

  if (currentTab !== ProfileTabs.LikedSongs) return;

  return (
    <div id="upload-list">
      {favouritesError ? (
        <ErrorBanner className="fade-in mb-2 mx-3">
          {favouritesError}
        </ErrorBanner>
      ) : null}
      {favouritesError === null && !loading && favourites.length < 1 ? (
        <InfoBanner className="fade-in mb-2 mx-3">
          {viewingOwnProfile ? (
            <div className="d-flex flex-row align-items-center justify-content-center">
              <Image src={wavePulse} height={25} width={25} />
              <span className="ml-2">
                You haven't liked any tracks! Start listening to some local
                artists and find someone new.
              </span>
            </div>
          ) : (
            <div className="d-flex flex-row align-items-center justify-content-center">
              <Image src={wavePulse} height={30} width={30} />
              <span className="ml-1">
                This account hasn't liked any tracks yet.
              </span>
            </div>
          )}
        </InfoBanner>
      ) : null}
      <InfiniteScroll
        dataLength={favourites.length} //This is important field to render the next data
        next={async () => await getMoreFavourites()}
        hasMore={canLoadMore}
        loader={<></>}
      >
        {favourites.map((track, index) => (
          <div key={index} className="fade-in p-2 track-container">
            <Track
              track={track}
              artistName={track.artistName}
              artistMemberId={track.artistMemberId}
              tracks={favourites}
              setTracks={setFavourites}
              canLoadMore={canLoadMore}
              playlistType={PlaylistTypes.Favourites}
              listeningProfileMemberId={profileDetails.memberId}
              viewingOwnProfile={viewingOwnProfile}
            />
          </div>
        ))}
      </InfiniteScroll>
      {loading ? (
        <div className="h-100 d-flex justify-content-center align-self-center">
          <InPageLoadingComponent height={80} width={80} />
        </div>
      ) : null}
    </div>
  );
};

export default Favourites;
