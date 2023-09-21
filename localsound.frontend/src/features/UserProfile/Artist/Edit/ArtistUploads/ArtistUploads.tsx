import { useState } from "react";
import { UserModel } from "../../../../../app/model/dto/user.model";
import ArtistUploadsList from "./ArtistUploadsList";
import ArtistUploadForm from "./ArtistUploadForm";

interface Props {
  userDetails: UserModel;
}

const ArtistUploads = ({ userDetails }: Props) => {
  const [uploading, setUploading] = useState(false);

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
