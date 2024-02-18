import { useState } from "react";
import userImg from "../../../assets/placeholder.png";
import { Image } from "semantic-ui-react";
import ImageCropper from "../../../common/components/Cropper/ImageCropper";
import { CropTypes } from "../../../app/model/enums/cropTypes";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import agent from "../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { handleUpdateUserProfilePhoto } from "../../../app/redux/actions/userSlice";
import { handleUpdateProfilePhoto } from "../../../app/redux/actions/pageDataSlice";
import ErrorBanner from "../../banner/ErrorBanner";
import { State } from "../../../app/model/redux/state";

const UpdateProfilePhoto = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails)!;
  const [file, setFile] = useState<File | null>(null);
  const [updatingProfilePhoto, setUpdatingProfilePhoto] = useState(false);
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>();
  const [submittingPhotoUpdate, setSubmittingPhotoUpdate] = useState(false);
  const userPhoto = userDetails!.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
  );
  const dispatch = useDispatch();

  const onFileUpload = async (file: Blob) => {
    const formData = new FormData();

    if (file) {
      formData.append("fileName", file.name);
      formData.append("formFile", file);
      formData.append("fileExt", `.png`);

      try {
        setSubmittingPhotoUpdate(true);
        setPhotoUpdateError(null);
        var result = await agent.Account.uploadProfileImage(
          userDetails?.memberId!,
          formData,
          AccountImageTypes.ProfileImage
        );

        dispatch(handleUpdateUserProfilePhoto(result));
        dispatch(handleUpdateProfilePhoto(result));
        cancelCrop();
      } catch (err: any) {
        setPhotoUpdateError(err);
        setSubmittingPhotoUpdate(false);
      }
    }
  };

  const cancelCrop = () => {
    setFile(null);
    setUpdatingProfilePhoto(false);
    setSubmittingPhotoUpdate(false);
  };

  return (
    <div className="d-flex flex-column px-3">
      <div className="d-flex mb-1">
        <p className="form-label">PROFILE PHOTO</p>
      </div>
      <div
        id="profile-photo-update"
        className="d-flex justify-content-center flex-column align-content-center mb-4"
      >
        {!updatingProfilePhoto ? (
          <>
            <Image
              src={
                !updatingProfilePhoto && userPhoto
                  ? userPhoto.accountImageUrl
                  : !updatingProfilePhoto && !userPhoto
                  ? userImg
                  : updatingProfilePhoto
                  ? URL.createObjectURL(file!)
                  : null
              }
              size="medium"
              circular
              className="align-self-center mb-2"
              style={{ height: "250px", width: "250px" }}
            />
            <label
              htmlFor="profilePhotoInput"
              className="btn mt-2 white-button align-self-center w-fit-content align-self-center px-5"
            >
              <h4>Update profile photo</h4>
            </label>
            <input
              type="file"
              id="profilePhotoInput"
              style={{ display: "none" }}
              onChange={(event) => {
                if (event && event.target && event.target.files) {
                  setFile(event.target.files[0]);
                  setUpdatingProfilePhoto(true);
                }
              }}
            />
          </>
        ) : file ? (
          <ImageCropper
            file={file}
            onFileUpload={onFileUpload}
            cancelCrop={cancelCrop}
            cropType={CropTypes.Circle}
            submittingPhoto={submittingPhotoUpdate}
          />
        ) : null}
      </div>
      {photoUpdateError ? <ErrorBanner children={photoUpdateError} /> : null}
    </div>
  );
};

export default UpdateProfilePhoto;
