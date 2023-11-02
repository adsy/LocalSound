import { UserModel } from "../../../../app/model/dto/user.model";
import { useEffect, useRef, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";
import { debounce } from "lodash";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";

interface Props {
  userDetails: UserModel;
  onUploads: boolean;
  tracks: ArtistTrackUploadModel[];
  setTracks: (tracks: ArtistTrackUploadModel[]) => void;
}

const ArtistUploadsList = ({
  userDetails,
  onUploads,
  tracks,
  setTracks,
}: Props) => {
  const [page, setPage] = useState(0);
  const listRef = useRef<HTMLDivElement>(null);
  const [loading, setLoading] = useState(false);
  const [canLoadMore, setCanLoadMore] = useState(true);

  window.onscroll = debounce(() => {
    if (listRef?.current) {
      if (
        onUploads &&
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

  useEffect(() => {
    (async () => {
      if (onUploads && canLoadMore) {
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
  }, [page, onUploads]);

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
    </div>
  );
};

export default ArtistUploadsList;
