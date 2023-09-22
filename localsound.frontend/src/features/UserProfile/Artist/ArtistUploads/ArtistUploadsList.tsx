import { Button } from "react-bootstrap";
import { UserModel } from "../../../../app/model/dto/user.model";
import { useEffect, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import WaveForm from "../../../../common/components/WaveForm/WaveForm";

interface Props {
  userDetails: UserModel;
  uploading: boolean;
  setUploading: (uploading: boolean) => void;
}

const ArtistUploadsList = ({ userDetails, uploading, setUploading }: Props) => {
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
      <div>upload list</div>
      <Button onClick={() => setUploading(!uploading)} className="black-button">
        Upload track
      </Button>

      {tracks.map((track, index) => (
        <div key={index}>
          <WaveForm track={track} />
        </div>
      ))}
    </>
  );
};

export default ArtistUploadsList;
