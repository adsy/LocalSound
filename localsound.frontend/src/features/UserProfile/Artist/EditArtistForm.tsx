import { Col } from "react-bootstrap";
import { Divider } from "semantic-ui-react";
import MyAddressInput from "../../../common/form/MyAddressInput";
import MyTextInput from "../../../common/form/MyTextInput";
import { UpdateArtistModel } from "../../../app/model/dto/update-artist";
import MyTextArea from "../../../common/form/MyTextArea";

interface Props {
  disabled?: boolean;
  setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void;
  setFieldTouched: (
    field: string,
    isTouched?: boolean,
    shouldValidate?: boolean
  ) => void;
  setAddressError: (addressError: boolean) => void;
  values: UpdateArtistModel;
}

const EditArtistForm = (props: Props) => {
  const { disabled, setFieldValue, setFieldTouched, setAddressError, values } =
    props;

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
    <div id="edit-form" className="d-flex flex-column px-4">
      <h3 className="mt-3">Artist details</h3>
      <div className="d-flex flex-row justify-content-between artist-details-row">
        <div className="d-flex flex-column w-50">
          <div className="d-flex">
            <p className="form-label">ARTIST NAME</p>
          </div>
          <MyTextInput name="name" placeholder="" disabled={disabled} />
          <div className="d-flex">
            <p className="form-label">PROFILE URL</p>
          </div>
          <MyTextInput name="profileUrl" placeholder="" disabled={disabled} />
        </div>
        <div className=" w-50">
          <div className="d-flex">
            <p className="form-label">ABOUT</p>
          </div>
          <MyTextArea name="aboutSection" placeholder="" rows={5} />
        </div>
      </div>
      <Divider />
      <h3 className="mt-2">Contact details</h3>
      <div className="d-flex flex-row justify-content-between artist-details-row">
        <div className="d-flex flex-column w-50">
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
        </div>
        <div className=" w-50">
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
        </div>
      </div>

      <Divider />

      <h3 className="mt-2">Social profiles</h3>
      <div className="d-flex">
        <p className="form-label">SOUNDCLOUD PROFILE</p>
      </div>
      <MyTextInput name="soundcloudUrl" placeholder="" />

      <div className="d-flex">
        <p className="form-label">SPOTIFY PROFILE</p>
      </div>
      <MyTextInput name="spotifyUrl" placeholder="" />
      <div className="d-flex">
        <p className="form-label">YOUTUBE PROFILE</p>
      </div>
      <MyTextInput name="youtubeUrl" placeholder="" />
      <Divider />
      <h3 className="mt-2">Genres</h3>
      <Divider />
      <h3 className="mt-2">Profile image</h3>
      <Divider />
      <h3 className="mt-2">Cover photo</h3>
    </div>
  );
};

export default EditArtistForm;
