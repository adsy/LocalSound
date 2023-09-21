import { useEffect, useState } from "react";
import { UserModel } from "../../../../../app/model/dto/user.model";
import agent from "../../../../../api/agent";
import { Button, Form } from "react-bootstrap";
import { TrackUploadSASModel } from "../../../../../app/model/dto/track-upload-sas.model";
import { BlobServiceClient, BlockBlobUploadOptions } from "@azure/storage-blob";
import ErrorBanner from "../../../../../common/banner/ErrorBanner";
import { Formik } from "formik";
import SuccessBanner from "../../../../../common/banner/SuccessBanner";

interface Props {
  userDetails: UserModel;
  uploading: boolean;
  setUploading: (uploading: boolean) => void;
}

const ArtistUploadForm = ({ userDetails, uploading, setUploading }: Props) => {
  const [file, setFile] = useState<File | null>(null);
  const [trackImage, setTrackImage] = useState<File | null>(null);
  const [uploadData, setUploadData] = useState<TrackUploadSASModel | null>(
    null
  );
  const [uploadTrackSuccess, setUploadTrackSuccess] = useState(false);
  const [uploadTrackError, setUploadTrackError] = useState(false);
  const [uploadDataError, setUploadDataError] = useState(false);

  useEffect(() => {
    (async () => {
      try {
        var result = await agent.Tracks.getTrackData(userDetails.memberId);
        setUploadData(result);
      } catch (err) {
        setUploadDataError(true);
      }
    })();
  }, []);

  const [progress, setProgress] = useState(0);

  const uploadBlob = async () => {
    const blobService = new BlobServiceClient(uploadData!.sasUri);

    var test = blobService.getContainerClient(uploadData!.containerName);

    return await test.uploadBlockBlob(
      uploadData!.uploadLocation,
      file!,
      file!.size,
      {
        blockSize: 4 * 1024 * 1024,
        concurrency: 20,
        onProgress: (ev) => {
          console.log(`you have upload ${ev.loadedBytes} bytes`);
          setProgress((ev.loadedBytes / file!.size) * 100);
        },
      } as BlockBlobUploadOptions
    );
  };

  const formValuesUntouched = () => {
    //TODO: add checks
    return false;
  };

  return (
    <>
      {uploadDataError ? (
        <ErrorBanner>
          There was an error getting your upload data, please refresh the page
          and try again..
        </ErrorBanner>
      ) : null}

      <div className="fade-in pb-4 mt-4">
        <div className="w-100 fade-in">
          <Formik
            initialValues={{
              trackName: "",
              trackDescription: "",
              genre: "",
            }}
            onSubmit={async (values, { setStatus }) => {
              try {
                var uploadResult = await uploadBlob();

                console.log(uploadResult.response);

                var formData = new FormData();

                await agent.Tracks.uploadTrackSupportingData(
                  userDetails.memberId,
                  uploadData!.trackId,
                  formData
                );

                setUploadTrackSuccess(true);
              } catch (err) {
                //TODO: Do something with error

                setUploadTrackError(true);
              }
            }}
            // validationSchema={} // TODO: Need to add validation
          >
            {({
              values,
              handleSubmit,
              isSubmitting,
              isValid,
              status,
              submitForm,
            }) => {
              const disabled =
                !isValid || isSubmitting || formValuesUntouched(values);
              return (
                <Form
                  className="ui form error fade-in"
                  onSubmit={handleSubmit}
                  autoComplete="off"
                >
                  <div className="form-body"></div>
                  {status?.error ? (
                    <ErrorBanner className="fade-in mb-0 mx-3">
                      {status.error}
                    </ErrorBanner>
                  ) : null}
                  {uploadTrackSuccess ? (
                    <SuccessBanner className="fade-in mb-0 mx-3">
                      Your track has been successfully updated. Go back to your
                      track list to check it out.
                    </SuccessBanner>
                  ) : null}
                  <div className="px-3 mt-3">
                    {!isSubmitting ? (
                      <Button
                        className={`black-button w-100 align-self-center`}
                        disabled={disabled || uploadDataError}
                        onClick={() => submitForm()}
                      >
                        <h4>Upload track</h4>
                      </Button>
                    ) : (
                      <h1>{progress}</h1>
                    )}
                  </div>
                </Form>
              );
            }}
          </Formik>
        </div>
      </div>

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
    </>
  );
};

export default ArtistUploadForm;
