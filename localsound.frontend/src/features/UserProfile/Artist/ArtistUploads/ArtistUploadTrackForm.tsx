import { useEffect, useRef, useState } from "react";
import { UserModel } from "../../../../app/model/dto/user.model";
import agent from "../../../../api/agent";
import { Button, Form, ProgressBar } from "react-bootstrap";
import { TrackUploadSASModel } from "../../../../app/model/dto/track-upload-sas.model";
import { BlockBlobUploadOptions, BlobClient } from "@azure/storage-blob";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import { Formik } from "formik";
import SuccessBanner from "../../../../common/banner/SuccessBanner";
import ArtistUploadsTrackSelection from "./ArtistUploadsTrackSelection";
import MyTextInput from "../../../../common/form/MyTextInput";
import MyTextArea from "../../../../common/form/MyTextArea";
import SearchGenreTypes from "../Edit/Search/SearchGenreTypes";
import { GenreModel } from "../../../../app/model/dto/genre.model";

interface Props {
  userDetails: UserModel;
  uploading: boolean;
  setUploading: (uploading: boolean) => void;
}

const ArtistUploadForm = ({ userDetails, uploading, setUploading }: Props) => {
  const [file, setFile] = useState<File | null>(null);
  const [trackExt, setTrackExt] = useState<string | null>(null);
  const [trackImage, setTrackImage] = useState<File | null>(null);
  const [uploadData, setUploadData] = useState<TrackUploadSASModel | null>(
    null
  );
  const [uploadTrackSuccess, setUploadTrackSuccess] = useState(false);
  const [uploadTrackError, setUploadTrackError] = useState(false);
  const [uploadDataError, setUploadDataError] = useState(false);
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
  const [dps, setDps] = useState<{ [x: string]: any } | null>(null);
  const [trackProgress, setTrackProgress] = useState(0);
  // const [waveformProgress, setWaveformProgress] = useState(0);

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

  const uploadBlob = async () => {
    var trackUrl = `${uploadData?.accountUrl}/${uploadData?.containerName}/${uploadData?.uploadLocation}.${trackExt}`;
    var url = `${trackUrl}?${uploadData?.sasToken}`;

    const blobClient = new BlobClient(url);

    blobClient.getProperties();

    var client = blobClient.getBlockBlobClient();

    return await client.upload(file!, file!.size, {
      blockSize: 4 * 1024 * 1024,
      concurrency: 20,
      onProgress: (ev) => {
        setTrackProgress((ev.loadedBytes / file!.size) * 100);
      },
    } as BlockBlobUploadOptions);
  };

  // const uploadWaveForm = async () => {
  //   var trackUrl = `${uploadData?.accountUrl}/${uploadData?.containerName}/${uploadData?.uploadLocation}.json`;
  //   var url = `${trackUrl}?${uploadData?.sasToken}`;

  //   const blobClient = new BlobClient(url);

  //   blobClient.getProperties();

  //   var client = blobClient.getBlockBlobClient();

  //   var dpsString = JSON.stringify({
  //     data: dps,
  //   });

  //   return await client.upload(dpsString, dpsString.length, {
  //     blockSize: 4 * 1024 * 1024,
  //     concurrency: 20,
  //     onProgress: (ev) => {
  //       setWaveformProgress((ev.loadedBytes / file!.size) * 100);
  //     },
  //   } as BlockBlobUploadOptions);
  // };

  const formValuesUntouched = (values: any) => {
    //TODO: add checks
    return false;
  };

  const getDuration = async (file: File): Promise<number> => {
    const url = URL.createObjectURL(file);

    return new Promise((resolve) => {
      const audio = document.createElement("audio");
      audio.muted = true;
      const source = document.createElement("source");
      source.src = url;
      audio.preload = "metadata";
      audio.appendChild(source);
      audio.onloadedmetadata = function () {
        resolve(audio.duration);
      };
    });
  };

  return (
    <div id="track-upload">
      {uploadDataError ? (
        <ErrorBanner>
          There was an error getting your upload data, please refresh the page
          and try again..
        </ErrorBanner>
      ) : null}

      {!file ? (
        <ArtistUploadsTrackSelection
          setFile={setFile}
          setTrackExt={setTrackExt}
          setDps={setDps}
        />
      ) : !uploadTrackSuccess ? (
        <div className="fade-in pb-4 mt-4">
          <div className="w-100 fade-in">
            <Formik
              initialValues={{
                trackName: "",
                trackDescription: "",
              }}
              onSubmit={async (values, { setStatus }) => {
                try {
                  if (file && trackImage) {
                    // await uploadWaveForm();
                    await uploadBlob();

                    var duration = await getDuration(file);

                    var trackImageExt = trackImage.name.split(/[.]+/).pop();

                    var formData = new FormData();
                    formData.append("trackName", values.trackName);
                    formData.append(
                      "trackDescription",
                      values.trackDescription
                    );
                    formData.append(
                      "trackImage",
                      trackImage!,
                      trackImage?.name
                    );
                    formData.append("trackImageExt", `.${trackImageExt}`);
                    formData.append("trackFileExt", `.${trackExt}`);
                    formData.append(
                      "fileLocation",
                      uploadData!.uploadLocation + `.${trackExt}`
                    );
                    formData.append(
                      "trackUrl",
                      `${uploadData?.accountUrl}/${uploadData?.containerName}/${uploadData?.uploadLocation}.${trackExt}`
                    );
                    formData.append(
                      "waveformUrl",
                      `${uploadData?.accountUrl}/${uploadData?.containerName}/${uploadData?.uploadLocation}.json`
                    );

                    selectedGenres.forEach((genre: GenreModel, index) => {
                      formData.append(
                        `genres[${index}].genreId`,
                        genre.genreId
                      );
                      formData.append(
                        `genres[${index}].genreName`,
                        genre.genreName
                      );
                    });

                    formData.append("duration", `${duration}`);

                    await agent.Tracks.uploadTrackSupportingData(
                      userDetails.memberId,
                      uploadData!.trackId,
                      formData
                    );

                    setUploadTrackSuccess(true);
                  }
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
                    <div className="form-body">
                      <div id="edit-form" className="d-flex flex-column">
                        <div className="d-flex flex-row flex-wrap justify-content-between">
                          <div className="d-flex flex-column col-12 col-md-6 px-3">
                            <div className="mb-3">
                              <div className="d-flex">
                                <p className="form-label">TRACK NAME</p>
                              </div>
                              <MyTextInput
                                name="trackName"
                                placeholder=""
                                disabled={disabled}
                              />
                            </div>
                            <div className="mb-3">
                              <div className="d-flex">
                                <p className="form-label">TRACK DESCRIPTION</p>
                              </div>
                              <MyTextArea
                                name="trackDescription"
                                placeholder=""
                                rows={5}
                              />
                            </div>
                            <div className="d-flex">
                              <p className="form-label">GENRES</p>
                            </div>
                            <SearchGenreTypes
                              selectedGenres={selectedGenres}
                              setSelectedGenres={setSelectedGenres}
                              placeholder={
                                "Search for a genre to add to your track"
                              }
                            />
                          </div>
                          <div className="d-flex flex-column col-12 col-md-6 px-3">
                            <label
                              htmlFor="trackUpload"
                              className="btn black-button fade-in-out"
                            >
                              <h4>Select track photo</h4>
                            </label>
                            <input
                              type="file"
                              accept=".jpg,.png,.jpeg"
                              id="trackUpload"
                              style={{ display: "none" }}
                              onChange={async (event) => {
                                if (event?.target?.files) {
                                  setTrackImage(event.target.files[0]);
                                }
                              }}
                            />
                          </div>
                        </div>
                      </div>
                    </div>
                    {status?.error ? (
                      <ErrorBanner className="fade-in mb-0 mx-3">
                        {status.error}
                      </ErrorBanner>
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
                        <ProgressBar>
                          {/* <ProgressBar
                            className="waveform-progress"
                            animated
                            now={waveformProgress}
                          /> */}
                          <ProgressBar
                            className="track-progress"
                            animated
                            now={trackProgress}
                          />
                        </ProgressBar>
                      )}
                    </div>
                  </Form>
                );
              }}
            </Formik>
          </div>
        </div>
      ) : (
        <>
          <SuccessBanner className="fade-in mb-0 mx-3">
            Your track has been successfully updated. Go back to your track list
            to check it out.
          </SuccessBanner>
          <Button>Go back to track list</Button>
        </>
      )}
    </div>
  );
};

export default ArtistUploadForm;
