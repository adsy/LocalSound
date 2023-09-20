import { useEffect, useState } from "react";
import { FileChunkSplitter } from "../../../../../util/FileChunkSplitter";
import { UserModel } from "../../../../../app/model/dto/user.model";
import { v4 as uuidv4 } from "uuid";
import agent from "../../../../../api/agent";

interface Props {
  userDetails: UserModel;
}

const ArtistUploads = ({ userDetails }: Props) => {
  const [file, setFile] = useState<File | null>(null);
  var splitter = new FileChunkSplitter();

  useEffect(() => {
    if (file) {
      var errorUploading = false;
      var partialTrackId = uuidv4();

      splitter
        .uploadFile(file, userDetails.memberId, partialTrackId)
        .catch((e) => {
          console.log(e);
          errorUploading = true;
        })
        .finally(() => {
          if (errorUploading) {
            // trigger clean up call
            // TODO: display message saying upload failed
          } else {
            agent.Tracks.completeUpload(userDetails.memberId, partialTrackId);
          }
        });
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
