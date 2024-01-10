import TextInput from "../../../../../common/form/TextInput";
import AddressInput from "../../../../../common/form/AddressInput";
import { UpdateArtistPersonalDetailsModel } from "../../../../../app/model/dto/update-artist-personal.model";
import TextArea from "../../../../../common/form/TextArea";
import { Image } from "semantic-ui-react";
import { AccountImageTypes } from "../../../../../app/model/enums/accountImageTypes";
import userImg from "../../../../../assets/placeholder.png";
import { useState } from "react";
import agent from "../../../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { handleUpdateUserProfilePhoto } from "../../../../../app/redux/actions/userSlice";
import { State } from "../../../../../app/model/redux/state";
import { CropTypes } from "../../../../../app/model/enums/cropTypes";
import ImageCropper from "../../../../../common/components/Cropper/ImageCropper";
import { handleResetModal } from "../../../../../app/redux/actions/modalSlice";
import ErrorBanner from "../../../../../common/banner/ErrorBanner";
import { handleUpdateProfilePhoto } from "../../../../../app/redux/actions/pageDataSlice";

interface Props {
  disabled?: boolean;
  values: UpdateArtistPersonalDetailsModel;
  setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void;
  setFieldTouched: (
    field: string,
    isTouched?: boolean,
    shouldValidate?: boolean
  ) => void;
  setAddressError: (addressError: boolean) => void;
}

const EditArtistDetailsForm = ({
  disabled,
  setFieldValue,
  setFieldTouched,
  setAddressError,
  values,
}: Props) => {
  const [file, setFile] = useState<File | null>(null);
  const [updatingProfilePhoto, setUpdatingProfilePhoto] = useState(false);
  const [photoUpdateError, setPhotoUpdateError] = useState<string | null>();
  const dispatch = useDispatch();
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [submittingPhotoUpdate, setSubmittingPhotoUpdate] = useState(false);
  const userPhoto = userDetails!.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
  );

  const handleMobileNumberChange = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.FocusEvent<HTMLInputElement, Element>
  ): void => {
    setFieldTouched("phoneNumber", true);
    let value = e.target.value;

    if (values.phoneNumber.length > value.length) {
      value = value.trim();
      setFieldValue("phoneNumber", value);
    } else {
      if (new RegExp("\\D").test(value[value.length - 1])) {
        return;
      }

      if (value.length > 12) {
        setFieldValue("phoneNumber", value.slice(0, 12));
      } else if (values.phoneNumber.length < value.length) {
        if (value.length === 4 || value.length === 5) {
          value = value.slice(0, 4) + " " + value.slice(4);
        }

        if (value.length === 9 || value.length === 10) {
          value = value.slice(0, 8) + " " + value.slice(8);
        }
        setFieldValue("phoneNumber", value);
      }
    }
  };

  const onFileUpload = async (file: Blob) => {
    const formData = new FormData();

    if (file) {
      formData.append("fileName", file.name);
      formData.append("formFile", file);
      formData.append("fileExt", `.png`);

      try {
        setSubmittingPhotoUpdate(true);
        setPhotoUpdateError(null);
        var result = await agent.Profile.uploadProfileImage(
          userDetails?.memberId!,
          formData,
          AccountImageTypes.ProfileImage
        );

        dispatch(handleUpdateUserProfilePhoto(result));
        dispatch(handleUpdateProfilePhoto(result));
        dispatch(handleResetModal());
      } catch (err: any) {
        setPhotoUpdateError(err);
        setSubmittingPhotoUpdate(false);
      }
    }
  };

  const cancelCrop = () => {
    setFile(null);
    setUpdatingProfilePhoto(false);
  };

  return (
    <div id="edit-form" className="d-flex flex-column">
      <div className="d-flex flex-row flex-wrap justify-content-between">
        <div className="d-flex flex-column col-12 col-md-6 px-3">
          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">ARTIST NAME</p>
            </div>
            <TextInput name="name" placeholder="" disabled={disabled} />
          </div>
          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">PROFILE URL</p>
            </div>
            <TextInput name="profileUrl" placeholder="" disabled={disabled} />
          </div>
          <div className="d-flex">
            <p className="form-label">MOBILE NUMBER</p>
          </div>
          <TextInput
            name="phoneNumber"
            placeholder=""
            className="mb-2"
            disabled={disabled}
            onChange={(e) => handleMobileNumberChange(e)}
            onBlur={(e) => handleMobileNumberChange(e)}
          />

          <div className="d-flex">
            <p className="form-label">ADDRESS</p>
          </div>
          <AddressInput
            name="address"
            placeholder=""
            setFieldValue={setFieldValue}
            setFieldTouched={setFieldTouched}
            setAddressError={setAddressError}
            disabled={disabled}
            preselectedAddress={values.address}
          />
          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">SOUNDCLOUD PROFILE</p>
            </div>
            <TextInput name="soundcloudUrl" placeholder="" />
          </div>

          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">SPOTIFY PROFILE</p>
            </div>
            <TextInput name="spotifyUrl" placeholder="" />
          </div>

          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">YOUTUBE PROFILE</p>
            </div>
            <TextInput name="youtubeUrl" placeholder="" />
          </div>
        </div>
        <div className="col-12 col-md-6 px-3 mb-3">
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
          {photoUpdateError ? (
            <ErrorBanner children={photoUpdateError} />
          ) : null}
          <div className="d-flex">
            <p className="form-label">ABOUT</p>
          </div>
          <TextArea name="aboutSection" placeholder="" rows={5} />
        </div>
      </div>
    </div>
  );
};

export default EditArtistDetailsForm;
