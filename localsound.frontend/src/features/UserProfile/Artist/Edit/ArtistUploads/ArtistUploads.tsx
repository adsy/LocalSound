import { useEffect, useState } from "react";
import { FileChunkSplitter } from "../../../../../util/FileChunkSplitter";
import { UserModel } from "../../../../../app/model/dto/user.model";

interface Props {
  userDetails: UserModel;
}

const ArtistUploads = ({ userDetails }: Props) => {
  const [file, setFile] = useState<File | null>(null);
  var splitter = new FileChunkSplitter();

  useEffect(() => {
    if (file) {
      splitter.uploadFile(file, userDetails.memberId);
    }
  }, [file]);

  return (
    <>
      <label htmlFor="trackUpload" className="btn black-button fade-in-out">
        <h4>Update track</h4>
      </label>
      <input
        type="file"
        id="trackUpload"
        style={{ display: "none" }}
        onChange={(event) => {
          if (event && event.target && event.target.files) {
            setFile(event.target.files[0]);
          }
        }}
      />
    </>
  );
};

export default ArtistUploads;
