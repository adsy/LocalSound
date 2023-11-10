import { UserModel } from "../../../../app/model/dto/user.model";
import { useLayoutEffect, useRef, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";
import { debounce } from "lodash";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import { ArtistProfileTabs } from "../../../../app/model/enums/artistProfileTabTypes";

interface Props {
  userDetails: UserModel;
  currentTab: ArtistProfileTabs;
  tracks: ArtistTrackUploadModel[];
  setTracks: (tracks: ArtistTrackUploadModel[]) => void;
  viewingOwnProfile: boolean;
  canLoadMore: boolean;
  setCanLoadMore: (loadMore: boolean) => void;
}

const ArtistUploadsList = ({
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
  }, [page, currentTab]);

  return (
    <div ref={listRef}>
      {tracks.map((track, index) => (
        <div key={index} className="fade-in">
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
        <div className="d-flex flex-row justify-content-center mt-5">
          <h5>
            {viewingOwnProfile ? (
              <span>
                There are no tracks available. Click on the upload icon within
                your profile banner to start uploading a track.
              </span>
            ) : (
              <span>This artist has no tracks uploaded yet.</span>
            )}
          </h5>
        </div>
      ) : null}
    </div>
  );
};

export default ArtistUploadsList;
