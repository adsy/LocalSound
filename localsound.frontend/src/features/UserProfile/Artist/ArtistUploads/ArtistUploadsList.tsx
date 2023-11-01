import { UserModel } from "../../../../app/model/dto/user.model";
import { useEffect, useRef, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";
import { Button } from "react-bootstrap";
import { debounce } from "lodash";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";

interface Props {
  userDetails: UserModel;
  key: string;
}

const ArtistUploadsList = ({ userDetails, key }: Props) => {
  const [tracks, setTracks] = useState<ArtistTrackUploadModel[]>([]);
  const [page, setPage] = useState(0);
  const listRef = useRef<HTMLDivElement>(null);
  const [loading, setLoading] = useState(false);
  const [canLoadMore, setCanLoadMore] = useState(false);

  window.onscroll = debounce(() => {
    if (listRef?.current) {
      console.log(
        window.innerHeight +
          document.documentElement.scrollTop -
          listRef?.current?.offsetHeight
      );
      if (
        key == "uploads" &&
        !loading &&
        canLoadMore &&
        window.innerHeight +
          document.documentElement.scrollTop -
          listRef?.current?.offsetHeight <
          690
      ) {
        setPage(page + 1);
      }
    }
  }, 100);

  useEffect(() => {
    (async () => {
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
    })();
  }, [page]);

  return (
    <div ref={listRef}>
      {tracks.map((track, index) => (
        <div key={index} className="fade-in">
          <Track track={track} artistDetails={userDetails} />
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
