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
      {loggedInUser?.memberId == userDetails.memberId ? (
        <div className="d-flex flex-row justify-content-end align-items-center mb-4">
          {/* <h3 className="inverse mb-0">Uploads</h3> */}
          <Button
            onClick={() => setUploading(!uploading)}
            className="white-button d-flex flex-row align-content-center"
          >
            <h4 className="m-0">Upload track</h4>
            <h4 className="mt-0 mb-0 ml-2">
              <Icon name="upload" className="m-0" />
            </h4>
          </Button>
        </div>
      ) : null}
      {tracks.map((track, index) => (
        <div key={index}>
          <Track track={track} artistDetails={userDetails} />
        </div>
      ))}
    </>
  );
};

export default ArtistUploadsList;
