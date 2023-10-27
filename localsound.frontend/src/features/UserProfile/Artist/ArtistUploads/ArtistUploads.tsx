import { UserModel } from "../../../../app/model/dto/user.model";
import ArtistUploadsList from "./ArtistUploadsList";
import ArtistUploadForm from "./ArtistUploadTrackForm";

interface Props {
  userDetails: UserModel;
  uploading: boolean;
  setUploading: (uploading: boolean) => void;
}

const ArtistUploads = ({ userDetails, uploading, setUploading }: Props) => {
  return (
    <div>
      {!uploading ? (
        <ArtistUploadsList
          userDetails={userDetails}
          uploading={uploading}
          setUploading={setUploading}
        />
      ) : (
        <ArtistUploadForm
          userDetails={userDetails}
          uploading={uploading}
          setUploading={setUploading}
        />
      )}
    </div>
  );
};

export default ArtistUploads;
