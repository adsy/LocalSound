import { UserModel } from "../../../../app/model/dto/user.model";
import { useEffect, useLayoutEffect, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import { ProfileTabs } from "../../../../app/model/enums/ProfileTabTypes";
import { Image } from "react-bootstrap";
import wavePulse from "../../../../assets/wave-pulse-1-svgrepo-com.svg";
import SuccessBanner from "../../../../common/banner/SuccessBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../../app/model/redux/state";
import { handleResetUploadTrackState } from "../../../../app/redux/actions/pageOperationSlice";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import InfoBanner from "../../../../common/banner/InfoBanner";
import useFixMissingScroll from "../../../../common/hooks/UseLoadMoreWithoutScroll";
import InfiniteScroll from "react-infinite-scroll-component";
import { PlaylistTypes } from "../../../../app/model/enums/playlistTypes";

interface Props {
  profileDetails: UserModel;
  currentTab: ProfileTabs;
  tracks: ArtistTrackModel[];
  setTracks: (tracks: ArtistTrackModel[]) => void;
  viewingOwnProfile: boolean;
}

const UploadList = ({
  profileDetails,
  currentTab,
  tracks,
  setTracks,
  viewingOwnProfile,
}: Props) => {
  const [loading, setLoading] = useState(false);
  const [trackError, setTrackError] = useState<string | null>();
  const [canLoadMore, setCanLoadMore] = useState(true);
  const uploadState = useSelector(
    (state: State) => state.pageOperation.uploadTracks
  );
  const playerState = useSelector((state: State) => state.player);
  const dispatch = useDispatch();

  const getMoreTracks = async () => {
    if (canLoadMore && currentTab === ProfileTabs.Uploads) {
      try {
        setLoading(true);

        var trackId = tracks[tracks.length - 1]?.artistTrackId;

        var result = await agent.Tracks.getTracks(
          profileDetails!.memberId,
          PlaylistTypes.Uploads,
          trackId
        );

        setTracks([...tracks, ...result.trackList]);

        setCanLoadMore(result.canLoadMore);
      } catch (err: any) {
        setTrackError(err);
      }
      setLoading(false);
    }
  };

  useFixMissingScroll({
    hasMoreItems: canLoadMore,
    fetchMoreItems: () => getMoreTracks(),
  });

  useLayoutEffect(() => {
    (async () => {
      if (currentTab === ProfileTabs.Uploads) {
        await getMoreTracks();
      }
    })();

    return () => {
      setTracks([]);
      setCanLoadMore(true);
      if (
        uploadState.trackUploaded ||
        uploadState.trackUpdated ||
        uploadState.trackDeleted
      ) {
        dispatch(handleResetUploadTrackState());
      }
      setTrackError(null);
    };
  }, [currentTab]);

  if (currentTab !== ProfileTabs.Uploads) return <div></div>;

  return (
    <div id="upload-list">
      {uploadState.trackUploaded ? (
        <>
          <SuccessBanner className="fade-in mb-2 mx-3">
            Your track has been successfully uploaded.
          </SuccessBanner>
        </>
      ) : null}
      {uploadState.trackUpdated ? (
        <>
          <SuccessBanner className="fade-in mb-2 mx-3">
            Your track has been successfully updated.
          </SuccessBanner>
        </>
      ) : null}
      {uploadState.trackDeleted ? (
        <>
          <SuccessBanner className="fade-in mb-2 mx-3">
            Your track has been successfully deleted.
          </SuccessBanner>
        </>
      ) : null}
      {trackError ? (
        <ErrorBanner className="fade-in mb-2 mx-3">{trackError}</ErrorBanner>
      ) : null}
      {trackError === null && !loading && tracks.length < 1 ? (
        <InfoBanner className="fade-in mb-2 mx-3">
          {viewingOwnProfile ? (
            <div className="d-flex flex-row align-items-center justify-content-center">
              <Image src={wavePulse} height={25} width={25} />
              <div className="ml-2">
                <p className="mb-0">
                  <span className="ml-1">There are no tracks available.</span>{" "}
                  Click on the upload icon within your profile banner to start
                  uploading a track.
                </p>
              </div>
            </div>
          ) : (
            <p>
              <Image src={wavePulse} height={30} width={30} />
              <span className="ml-1">
                This artist has no tracks uploaded yet.
              </span>
            </p>
          )}
        </InfoBanner>
      ) : null}
      <InfiniteScroll
        dataLength={tracks.length}
        next={async () => await getMoreTracks()}
        hasMore={canLoadMore}
        loader={<></>}
      >
        {tracks.map((track, index) => (
          <div key={index} className="fade-in p-2 track-container">
            <Track
              track={track}
              artistName={track.artistName}
              artistMemberId={track.artistMemberId}
              tracks={tracks}
              setTracks={setTracks}
              canLoadMore={canLoadMore}
              playlistType={PlaylistTypes.Uploads}
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

export default UploadList;
