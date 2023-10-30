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
import userImg from "../../../../assets/icons/user.svg";
import { Image, Placeholder } from "semantic-ui-react";
import ImageCropper from "../../../../common/components/Cropper/ImageCropper";
import { CropTypes } from "../../../../app/model/enums/cropTypes";

interface Props {
  userDetails: UserModel;
}

const ArtistUploadForm = ({ userDetails }: Props) => {
  const [file, setFile] = useState<File | null>(null);
  const [trackExt, setTrackExt] = useState<string | null>(null);
  const [trackImage, setTrackImage] = useState<File | null>(null);
  const [croppedImage, setCroppedImage] = useState<Blob | null>(null);
  const [uploadData, setUploadData] = useState<TrackUploadSASModel | null>(
    null
  );
  const [uploadTrackSuccess, setUploadTrackSuccess] = useState(false);
  const [uploadTrackError, setUploadTrackError] = useState(false);
  const [uploadDataError, setUploadDataError] = useState(false);
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
  const [trackProgress, setTrackProgress] = useState(0);
  const [updatingTrackPhoto, setUpdatingTrackPhoto] = useState(false);

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

  const formValuesUntouched = (values: any) => {
    //TODO: add checks
    return false;
  };

  const onFileUpload = async (file: Blob) => {
    setCroppedImage(file);
    setUpdatingTrackPhoto(false);
  };

  const cancelCrop = () => {
    // setFile(null);
    setUpdatingTrackPhoto(false);
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
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">Upload track</h2>
      </div>
      <div id="track-upload" className="px-3">
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
                    if (file && croppedImage) {
                      // await uploadWaveForm();
                      await uploadBlob();

                      var duration = await getDuration(file);

                      var trackImageExt = ".jpg";

                      var formData = new FormData();
                      formData.append("trackName", values.trackName);
                      formData.append(
                        "trackDescription",
                        values.trackDescription
                      );

                      formData.append(
                        "trackImage",
                        croppedImage!,
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
                              <div className="mb-3">
                                <div className="d-flex">
                                  <p className="form-label">
                                    TRACK DESCRIPTION
                                  </p>
                                </div>
                                <MyTextArea
                                  name="trackDescription"
                                  placeholder=""
                                  rows={7}
                                />
                              </div>
                            </div>
                            <div className="d-flex flex-column col-12 col-md-6 px-3">
                              <div className="d-flex mb-1">
                                <p className="form-label">TRACK PHOTO</p>
                              </div>
                              {!updatingTrackPhoto ? (
                                <>
                                  <Image
                                    src={
                                      !croppedImage
                                        ? userImg
                                        : URL.createObjectURL(croppedImage!)
                                    }
                                    size="medium"
                                    className="align-self-center mb-2"
                                    style={{ borderRadius: "5px" }}
                                  />

                                  <label
                                    htmlFor="trackUpload"
                                    className="btn white-button fade-in-out w-fit-content align-self-center px-5"
                                  >
                                    <h4>Select photo</h4>
                                  </label>
                                  <input
                                    type="file"
                                    accept=".jpg,.png,.jpeg"
                                    id="trackUpload"
                                    style={{ display: "none" }}
                                    onChange={async (event) => {
                                      if (event?.target?.files) {
                                        setTrackImage(event.target.files[0]);
                                        setUpdatingTrackPhoto(true);
                                      }
                                    }}
                                  />
                                </>
                              ) : trackImage ? (
                                <ImageCropper
                                  file={trackImage}
                                  onFileUpload={onFileUpload}
                                  cancelCrop={cancelCrop}
                                  cropType={CropTypes.Square}
                                />
                              ) : null}
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
                            disabled={
                              disabled ||
                              uploadDataError ||
                              !file ||
                              !croppedImage
                            }
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
              Your track has been successfully updated. Go back to your track
              list to check it out.
            </SuccessBanner>
          </>
        )}
      </div>
    </div>
  );
};

export default ArtistUploadForm;
