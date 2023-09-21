import { useEffect, useState } from "react";
import { UserModel } from "../../../../../app/model/dto/user.model";
import agent from "../../../../../api/agent";
import { Button } from "react-bootstrap";
import { TrackUploadSASModel } from "../../../../../app/model/dto/track-upload-sas.model";
import {
  BlobServiceClient,
  BlockBlobParallelUploadOptions,
  BlockBlobUploadOptions,
} from "@azure/storage-blob";

interface Props {
  userDetails: UserModel;
}

const ArtistUploads = ({ userDetails }: Props) => {
  const [file, setFile] = useState<File | null>(null);
  const [trackImage, setTrackImage] = useState<File | null>(null);
  const [uploadData, setUploadData] = useState<TrackUploadSASModel | null>(
    null
  );
  const [progress, setProgress] = useState(0);

  const uploadBlob = async () => {
    const blobService = new BlobServiceClient(uploadData!.sasUri);

    var test = blobService.getContainerClient(uploadData!.containerName);

    test.uploadBlockBlob(uploadData!.uploadLocation, file!, file!.size, {
      blockSize: 4 * 1024 * 1024,
      concurrency: 20,
      onProgress: (ev) => {
        console.log(`you have upload ${ev.loadedBytes} bytes`);
        setProgress((ev.loadedBytes / file!.size) * 100);
      },
    } as BlockBlobUploadOptions);
  };

  const getUploadData = async () => {
    var result = await agent.Tracks.getTrackData(userDetails.memberId);
    setUploadData(result);
  };

  return (
    <>
      <Button onClick={async () => await getUploadData()}>
        Get upload data
      </Button>

      <label htmlFor="trackUpload" className="btn black-button fade-in-out">
        <h4>Upload track</h4>
      </label>
      <input
        type="file"
        id="trackUpload"
        style={{ display: "none" }}
        onChange={async (event) => {
          if (event && event.target && event.target.files) {
            setFile(event.target.files[0]);
          }
        }}
      />

      <Button onClick={async () => await uploadBlob()}>Upload</Button>

      {/* 
      <label htmlFor="imageUpload" className="btn black-button fade-in-out">
        <h4>Upload image</h4>
      </label>
      <input
        type="file"
        id="imageUpload"
        style={{ display: "none" }}
        onChange={(event) => {
          if (event && event.target && event.target.files) {
            setTrackImage(event.target.files[0]);
          }
        }}
      /> */}

      <h1>{progress}</h1>
    </>
  );
};

export default ArtistUploads;
