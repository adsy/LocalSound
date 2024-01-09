import { Formik } from "formik";
import { ArtistTrackUploadModel } from "../../../../app/model/dto/artist-track-upload.model";
import { UserModel } from "../../../../app/model/dto/user.model";
import { useState } from "react";
import { Button, Form, ProgressBar } from "react-bootstrap";
import TextInput from "../../../../common/form/TextInput";
import SearchGenreTypes from "../Edit/Search/SearchGenreTypes";
import { Image } from "semantic-ui-react";
import TextArea from "../../../../common/form/TextArea";
import ImageCropper from "../../../../common/components/Cropper/ImageCropper";
import { CropTypes } from "../../../../app/model/enums/cropTypes";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import agent from "../../../../api/agent";
import { GenreModel } from "../../../../app/model/dto/genre.model";
import { AccountImageTypes } from "../../../../app/model/enums/accountImageTypes";
import PlaceholderImg from "../../../../assets/placeholder.png";
import { useDispatch } from "react-redux";
import { handleResetModal } from "../../../../app/redux/actions/modalSlice";
import {
  handleTrackUpdated,
  handleTrackUploaded,
} from "../../../../app/redux/actions/pageOperationSlice";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";

interface Props {
  trackDetails: ArtistTrackUploadModel;
  tracks: ArtistTrackUploadModel[];
  setTracks: (tracks: ArtistTrackUploadModel[]) => void;
  userDetails: UserModel;
}

const EditTrackForm = ({
  trackDetails,
  userDetails,
  tracks,
  setTracks,
}: Props) => {
  const [editTrackError, setEditTrackError] = useState<string | null>();
  const [selectedGenres, setSelectedGenres] = useState([
    ...trackDetails.genres,
  ]);
  const [croppedImage, setCroppedImage] = useState<Blob | null>(null);
  const [updatingTrackPhoto, setUpdatingTrackPhoto] = useState(false);
  const [trackImage, setTrackImage] = useState<File | null>(null);
  const dispatch = useDispatch();

  const formValuesUntouched = (values: {
    trackName: string;
    trackDescription: string;
  }) => {
    if (
      JSON.stringify([...trackDetails.genres]) !==
      JSON.stringify([...selectedGenres])
    ) {
      return false;
    }
    if (values.trackName !== trackDetails.trackName) {
      return false;
    }
    if (values.trackDescription !== trackDetails.trackDescription) {
      return false;
    }
    if (croppedImage) {
      return false;
    }
    return true;
  };

  const onFileUpload = async (file: Blob) => {
    setCroppedImage(file);
    setUpdatingTrackPhoto(false);
  };

  const getDisplayImage = () => {
    if (!croppedImage) {
      var trackImg = trackDetails.trackImageUrl;

      if (trackImg) {
        return trackImg;
      }

      var profileImg = userDetails.images?.find(
        (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
      );
      if (profileImg != null) {
        return profileImg.accountImageUrl;
      }
      return PlaceholderImg;
    }

    return URL.createObjectURL(croppedImage);
  };

  const cancelCrop = () => {
    setUpdatingTrackPhoto(false);
  };

  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">Edit track details</h2>
      </div>
      <div className="fade-in pb-4 mt-4">
        <div className="w-100 fade-in">
          <Formik
            initialValues={{
              trackName: trackDetails.trackName,
              trackDescription: trackDetails.trackDescription,
            }}
            onSubmit={async (values, { setStatus }) => {
              dispatch(handleTrackUpdated(false));
              try {
                var trackImageExt = ".jpg";
                var formData = new FormData();

                formData.append("trackName", values.trackName);
                formData.append("trackDescription", values.trackDescription);

                if (croppedImage) {
                  formData.append(
                    "trackImage",
                    croppedImage!,
                    trackImage?.name
                  );
                  formData.append("trackImageExt", `${trackImageExt}`);
                }

                selectedGenres.forEach((genre: GenreModel, index) => {
                  formData.append(`genres[${index}].genreId`, genre.genreId);
                  formData.append(
                    `genres[${index}].genreName`,
                    genre.genreName
                  );
                });

                // Update track
                await agent.Tracks.editTrackSupportingDetails(
                  userDetails.memberId,
                  trackDetails!.artistTrackUploadId,
                  formData
                );

                // Get the updated track details so its updated in the upload list
                var updatedTrack = await agent.Tracks.getTrackDetails(
                  userDetails.memberId,
                  trackDetails!.artistTrackUploadId
                );

                var updatedTrackIndex = tracks.findIndex(
                  (x) =>
                    x.artistTrackUploadId === trackDetails.artistTrackUploadId
                );

                var trackList = [...tracks];
                trackList[updatedTrackIndex] = updatedTrack;

                setTracks(trackList);
                dispatch(handleTrackUpdated(true));
                dispatch(handleResetModal());
              } catch (err: any) {
                setEditTrackError(err);
              }
            }}
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
                            <TextInput
                              name="trackName"
                              placeholder=""
                              disabled={isSubmitting}
                            />
                          </div>
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">GENRES</p>
                            </div>
                            <SearchGenreTypes
                              selectedGenres={selectedGenres}
                              setSelectedGenres={setSelectedGenres}
                              placeholder={"Add up to 10 genres for your track"}
                            />
                          </div>
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">TRACK DESCRIPTION</p>
                            </div>
                            <TextArea
                              name="trackDescription"
                              placeholder=""
                              rows={6}
                              disabled={isSubmitting}
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
                                src={getDisplayImage()}
                                size="medium"
                                className="align-self-center mb-2"
                                style={{ borderRadius: "5px" }}
                              />

                              <label
                                htmlFor="trackUpload"
                                className="btn mt-2 white-button fade-in-out w-fit-content align-self-center px-5"
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
                  {editTrackError ? (
                    <ErrorBanner
                      children={editTrackError}
                      className="fade-in mb-0 mx-3"
                    ></ErrorBanner>
                  ) : null}

                  <div className="px-3 mt-3">
                    {!isSubmitting ? (
                      <Button
                        className={`black-button w-100 align-self-center`}
                        disabled={
                          disabled ||
                          !values.trackName ||
                          !values.trackDescription ||
                          selectedGenres.length == 0
                        }
                        onClick={() => submitForm()}
                      >
                        <h4>Update track</h4>
                      </Button>
                    ) : (
                      <InPageLoadingComponent />
                    )}
                  </div>
                </Form>
              );
            }}
          </Formik>
        </div>
      </div>
    </div>
  );
};

export default EditTrackForm;
