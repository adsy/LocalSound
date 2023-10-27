import { Button } from "react-bootstrap";
import { UserModel } from "../../../../app/model/dto/user.model";
import { useEffect, useState } from "react";
import agent from "../../../../api/agent";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import Track from "../../../../common/components/Track/Track";
import { Divider, Icon } from "semantic-ui-react";
import { useSelector } from "react-redux";
import { State } from "../../../../app/model/redux/state";

interface Props {
  userDetails: UserModel;
  uploading: boolean;
  setUploading: (uploading: boolean) => void;
}

const ArtistUploadsList = ({ userDetails, uploading, setUploading }: Props) => {
  const loggedInUser = useSelector((state: State) => state.user.userDetails);
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
