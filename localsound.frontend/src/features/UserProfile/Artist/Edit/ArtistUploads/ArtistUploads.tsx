import { useEffect, useState } from "react";
import { UserModel } from "../../../../../app/model/dto/user.model";
import agent from "../../../../../api/agent";
import { Button } from "react-bootstrap";
import { TrackUploadSASModel } from "../../../../../app/model/dto/track-upload-sas.model";
import {
  BlobServiceClient,
  BlockBlobParallelUploadOptions,
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

    var blob = test.getBlockBlobClient(uploadData!.uploadLocation);

    var maxConcurrency = 20; // max uploading concurrency
    var blockSize = 4 * 1024 * 1024; // the block size in the uploaded block blob

    var res = await blob.uploadBrowserData(file!, {
      blockSize: 4 * 1024 * 1024, // 4MB block size
      concurrency: 20, // 20 concurrency
      onProgress: (ev) => {
        console.log(`you have upload ${ev.loadedBytes} bytes`);
        setProgress((ev.loadedBytes / file!.size) * 100);
      },
    } as BlockBlobParallelUploadOptions);

    // await test
    //   .uploadBlockBlob(, file!, file!.size)
    //   .then((e) => {
    //     console.log(e);
    //   })
    //   .catch((e) => {
    //     console.log(e);
    //   })
    //   .finally(() => {
    //     console.log("here");
    //   });

    // var blobService = AzureStorageBlob..createBlobServiceWithSas(
    //   blobUri,
    //   sasToken
    // );

    // var file = $("#FileInput").get(0).files[0];

    // var customBlockSize =
    //   file.size > 1024 * 1024 * 32 ? 1024 * 1024 * 4 : 1024 * 512;
    // blobService.singleBlobPutThresholdInBytes = customBlockSize;

    // var finishedOrError = false;
    // var speedSummary = blobService.createBlockBlobFromBrowserFile(
    //   containerName,
    //   file.name,
    //   file,
    //   { blockSize: customBlockSize },
    //   function (error, result, response) {
    //     finishedOrError = true;
    //     if (error) {
    //       alert("Error");
    //     } else {
    //       // displayProcess(100);
    //     }
    //   }
    // );

    // function refreshProgress() {
    //   setTimeout(function () {
    //     if (!finishedOrError) {
    //       var process = speedSummary.getCompletePercent();
    //       // displayProcess(process);
    //       refreshProgress();
    //     }
    //   }, 200);
    // }

    // refreshProgress();
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
