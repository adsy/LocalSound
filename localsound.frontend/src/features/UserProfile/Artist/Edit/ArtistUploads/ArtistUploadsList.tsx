import { Button } from "react-bootstrap";
import { UserModel } from "../../../../../app/model/dto/user.model";

interface Props {
  userDetails: UserModel;
  uploading: boolean;
  setUploading: (uploading: boolean) => void;
}

const ArtistUploadsList = ({ userDetails, uploading, setUploading }: Props) => {
  return (
    <>
      <div>upload list</div>
      <Button onClick={() => setUploading(!uploading)} className="black-button">
        Upload track
      </Button>
    </>
  );
};

export default ArtistUploadsList;
