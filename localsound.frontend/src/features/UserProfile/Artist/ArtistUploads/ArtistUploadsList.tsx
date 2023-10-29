import { UserModel } from "../../../../app/model/dto/user.model";
import { useEffect, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";

interface Props {
  userDetails: UserModel;
}

const ArtistUploadsList = ({ userDetails }: Props) => {
  const [tracks, setTracks] = useState<ArtistTrackUploadModel[]>([]);

  useEffect(() => {
    (async () => {
      try {
        var result = await agent.Tracks.getArtistUploads(userDetails!.memberId);
        setTracks(result);
      } catch (err) {
        //TODO: Do something with error
      }
    })();
  }, []);

  return (
    <>
      {tracks.map((track, index) => (
        <div key={index}>
          <Track track={track} artistDetails={userDetails} />
        </div>
      ))}
    </>
  );
};

export default ArtistUploadsList;
