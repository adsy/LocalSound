import MyTextInput from "../../../../../common/form/MyTextInput";
import MyAddressInput from "../../../../../common/form/MyAddressInput";
import { UpdateArtistPersonalDetailsModel } from "../../../../../app/model/dto/update-artist-personal.model";
import MyTextArea from "../../../../../common/form/MyTextArea";
import { Image } from "semantic-ui-react";
import { UserModel } from "../../../../../app/model/dto/user.model";
import { AccountImageTypes } from "../../../../../app/model/enums/accountImageTypes";
import userImg from "../../../../../assets/icons/user.svg";
import { Button } from "react-bootstrap";

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
  userDetails: UserModel;
}

const EditArtistDetailsForm = ({
  disabled,
  setFieldValue,
  setFieldTouched,
  setAddressError,
  values,
  userDetails,
}: Props) => {
  const userPhoto = userDetails.images.find(
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

  return (
    <div id="edit-form" className="d-flex flex-column">
      <div className="d-flex flex-row flex-wrap justify-content-between">
        <div className="d-flex flex-column col-12 col-md-6 px-3">
          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">ARTIST NAME</p>
            </div>
            <MyTextInput name="name" placeholder="" disabled={disabled} />
          </div>
          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">PROFILE URL</p>
            </div>
            <MyTextInput name="profileUrl" placeholder="" disabled={disabled} />
          </div>
          <div className="d-flex">
            <p className="form-label">MOBILE NUMBER</p>
          </div>
          <MyTextInput
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
          <MyAddressInput
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
            <MyTextInput name="soundcloudUrl" placeholder="" />
          </div>

          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">SPOTIFY PROFILE</p>
            </div>
            <MyTextInput name="spotifyUrl" placeholder="" />
          </div>

          <div className="mb-3">
            <div className="d-flex">
              <p className="form-label">YOUTUBE PROFILE</p>
            </div>
            <MyTextInput name="youtubeUrl" placeholder="" />
          </div>
        </div>
        <div className="col-12 col-md-6 px-3 mb-3">
          <div className="d-flex mb-1">
            <p className="form-label">PROFILE PHOTO</p>
          </div>
          <div className="d-flex justify-content-center flex-column align-content-center mb-4">
            <Image
              src={userPhoto ? userPhoto.accountImageUrl : userImg}
              size="medium"
              circular
              className="align-self-center mb-2"
            />
            <Button
              className={`black-button w-100 align-self-center`}
              // onClick={() => submitForm()}
            >
              <h4>Upload profile photo</h4>
            </Button>
          </div>
          <div className="d-flex">
            <p className="form-label">ABOUT</p>
          </div>
          <MyTextArea name="aboutSection" placeholder="" rows={5} />
        </div>
      </div>
    </div>
  );
};

export default EditArtistDetailsForm;
