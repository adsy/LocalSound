import { UserModel } from "../../../../app/model/dto/user.model";
import { useLayoutEffect, useRef, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";
import { debounce } from "lodash";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import { ArtistProfileTabs } from "../../../../app/model/enums/artistProfileTabTypes";
import { Image } from "react-bootstrap";
import wavePulse from "../../../../assets/wave-pulse-1-svgrepo-com.svg";
import SuccessBanner from "../../../../common/banner/SuccessBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../../app/model/redux/state";
import {
  handleResetUploadTrackState,
  handleTrackUpdated,
  handleTrackUploaded,
} from "../../../../app/redux/actions/pageOperationSlice";

interface Props {
  userDetails: UserModel;
  currentTab: ArtistProfileTabs;
  tracks: ArtistTrackUploadModel[];
  setTracks: (tracks: ArtistTrackUploadModel[]) => void;
  viewingOwnProfile: boolean;
  canLoadMore: boolean;
  setCanLoadMore: (loadMore: boolean) => void;
}

const UploadList = ({
  userDetails,
  currentTab,
  tracks,
  setTracks,
  viewingOwnProfile,
  setCanLoadMore,
  canLoadMore,
}: Props) => {
  const [page, setPage] = useState(0);
  const listRef = useRef<HTMLDivElement>(null);
  const [loading, setLoading] = useState(false);
  const uploadState = useSelector(
    (state: State) => state.pageOperation.uploadTracks
  );
  const dispatch = useDispatch();

  window.onscroll = debounce(() => {
    if (listRef?.current) {
      if (
        currentTab === ArtistProfileTabs.Uploads &&
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
      if (currentTab === ArtistProfileTabs.Uploads && canLoadMore) {
        try {
          setLoading(true);
          var result = await agent.Tracks.getArtistUploads(
            userDetails!.memberId,
            page
          );
          setLoading(false);
          setTracks([...tracks, ...result.trackList]);
          setCanLoadMore(result.canLoadMore);
        } catch (err) {
          //TODO: Do something with error
        }
      }
    })();

    return () => {
      if (
        uploadState.trackUploaded ||
        uploadState.trackUpdated ||
        uploadState.trackDeleted
      ) {
        dispatch(handleResetUploadTrackState());
      }
    };
  }, [page, currentTab]);

  return (
    <div ref={listRef} id="upload-list">
      {uploadState.trackUploaded ? (
        <>
          <SuccessBanner className="fade-in mb-0 mx-3">
            Your track has been successfully uploaded.
          </SuccessBanner>
        </>
      ) : null}
      {uploadState.trackUpdated ? (
        <>
          <SuccessBanner className="fade-in mb-0 mx-3">
            Your track has been successfully updated.
          </SuccessBanner>
        </>
      ) : null}
      {uploadState.trackDeleted ? (
        <>
          <SuccessBanner className="fade-in mb-0 mx-3">
            Your track has been successfully deleted.
          </SuccessBanner>
        </>
      ) : null}
      {tracks.map((track, index) => (
        <div key={index} className="fade-in p-2 track-container">
          <Track
            track={track}
            artistDetails={userDetails}
            tracks={tracks}
            setTracks={setTracks}
          />
        </div>
      ))}
      {loading ? (
        <div className="h-100 mt-5 d-flex justify-content-center align-self-center">
          <InPageLoadingComponent height={80} width={80} />
        </div>
      ) : null}
      {!loading && tracks.length < 1 ? (
        <div className="d-flex flex-row justify-content-center mt-4">
          <div className="d-flex flex-column text-center align-items-center black-alert">
            {viewingOwnProfile ? (
              <div className="ml-2">
                <p className="mb-0">
                  <Image src={wavePulse} height={25} width={25} />
                  <span className="ml-1">
                    There are no tracks available.
                  </span>{" "}
                  Click on the upload icon within your profile banner to start
                  uploading a track.
                </p>
                <p></p>
              </div>
            ) : (
              <p>
                <Image src={wavePulse} height={30} width={30} />
                <span className="ml-1">
                  This artist has no tracks uploaded yet.
                </span>
              </p>
            )}
          </div>
        </div>
      ) : null}
    </div>
  );
};

export default UploadList;
