import { useLayoutEffect, useState } from "react";
import { UserModel } from "../../../app/model/dto/user.model";
import { ProfileTabs } from "../../../app/model/enums/ProfileTabTypes";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Image } from "semantic-ui-react";
import wavePulse from "../../../../assets/wave-pulse-1-svgrepo-com.svg";
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
  const [page, setPage] = useState(0);
  const [loading, setLoading] = useState(true);
  const [favourites, setFavourites] = useState<ArtistTrackUploadModel[]>([]);
  const [canLoadMore, setCanLoadMore] = useState(true);
  const [favouritesError, setFavouritesError] = useState<string | null>(null);
  const playerState = useSelector((state: State) => state.player);
  const dispatch = useDispatch();

  const getMoreFavourites = async () => {
    if (currentTab === ProfileTabs.LikedSongs) {
      try {
        setLoading(true);
        var result = await agent.Tracks.getTracks(
          profileDetails!.memberId,
          page,
          PlaylistTypes.Favourites
        );
        setFavourites([...favourites, ...result.trackList]);
        setCanLoadMore(result.canLoadMore);

        //TODO: Fix the player so it knows to get more artist tracks or get artists liked songs
        // if (profileDetails.profileUrl === playerState.listeningProfile) {
        //   dispatch(
        //     handleSetTrackList({
        //       trackList: [...tracks, ...result.trackList],
        //       page: page + 1,
        //       canLoadMore: result.canLoadMore,
        //     })
        //   );
        // }
      } catch (err: any) {
        setFavouritesError(err);
      }
      setLoading(false);
      setPage(page + 1);
    }
  };

  useFixMissingScroll({
    hasMoreItems: canLoadMore,
    fetchMoreItems: getMoreFavourites,
  });

  useLayoutEffect(() => {
    (async () => {
      if (currentTab === ProfileTabs.LikedSongs && canLoadMore) {
        try {
          setLoading(true);
          var result = await agent.Tracks.getTracks(
            profileDetails!.memberId,
            page,
            PlaylistTypes.Favourites
          );
          setFavourites([...favourites, ...result.trackList]);
          setCanLoadMore(result.canLoadMore);

          if (profileDetails.profileUrl === playerState.listeningProfile) {
            dispatch(
              handleSetTrackList({
                trackList: [...favourites, ...result.trackList],
                page: page + 1,
                canLoadMore: result.canLoadMore,
              })
            );
          }
        } catch (err: any) {
          setFavouritesError(err);
        }
        setLoading(false);
        setPage(page + 1);
      }

      //TODO: Fix this so playerState can handle liked songs or uploads
      // if (profileDetails.profileUrl !== playerState.listeningProfile) {
      //   if (currentTab === ProfileTabs.LikedSongs && canLoadMore) {
      //     try {
      //       setLoading(true);
      //       var result = await agent.Tracks.getLikedTracks(
      //         profileDetails.memberId,
      //         page
      //       );
      //       setFavourites([...favourites, ...result.trackList]);
      //       setCanLoadMore(result.canLoadMore);

      //       if (profileDetails.profileUrl === playerState.listeningProfile) {
      //         dispatch(
      //           handleSetTrackList({
      //             trackList: [...tracks, ...result.trackList],
      //             page: page + 1,
      //             canLoadMore: result.canLoadMore,
      //           })
      //         );
      //       }
      //     } catch (err: any) {
      //       setTrackError(err);
      //     }
      //     setLoading(false);
      //     setPage(page + 1);
      //   }
      // } else {
      //   setFavourites([...playerState.trackList]);
      //   setCanLoadMore(playerState.canLoadMore);
      //   setPage(playerState.page);
      // }
    })();

    //   return () => {
    //     if (
    //       uploadState.trackUploaded ||
    //       uploadState.trackUpdated ||
    //       uploadState.trackDeleted
    //     ) {
    //       dispatch(handleResetUploadTrackState());
    //     }
    //     setTrackError(null);
    //   };
  }, [currentTab]);

  //   useEffect(() => {
  //     if (
  //       playerState.trackList.length > 0 &&
  //       profileDetails.profileUrl === playerState.artistProfile
  //     ) {
  //       setTracks(playerState.trackList);
  //       setCanLoadMore(playerState.canLoadMore);
  //       setPage(playerState.page);
  //     }
  //   }, [playerState.trackList]);

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
              <div className="ml-2">
                <p className="mb-0">
                  <span className="ml-1">You haven't liked any tracks!</span>
                  Start listening to some local artists and find someone new.
                </p>
              </div>
            </div>
          ) : (
            <p>
              <Image src={wavePulse} height={30} width={30} />
              <span className="ml-1">
                This account hasn't liked any tracks yet.
              </span>
            </p>
          )}
        </InfoBanner>
      ) : null}
      <InfiniteScroll
        dataLength={favourites.length} //This is important field to render the next data
        next={() => getMoreFavourites()}
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
              page={page}
              playlistType={PlaylistTypes.Favourites}
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
