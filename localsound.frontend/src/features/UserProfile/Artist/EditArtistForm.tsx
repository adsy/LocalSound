import { Button, Col } from "react-bootstrap";
import { Divider } from "semantic-ui-react";
import MyAddressInput from "../../../common/form/MyAddressInput";
import MyTextInput from "../../../common/form/MyTextInput";
import { EditArtistModel } from "../../../app/model/dto/edit-artist.model";

interface Props {
  disabled?: boolean;
  setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void;
  setFieldTouched: (
    field: string,
    isTouched?: boolean,
    shouldValidate?: boolean
  ) => void;
  setAddressError: (addressError: boolean) => void;
  values: EditArtistModel;
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
    <div className="d-flex flex-row flex-wrap p-2 register-form">
      <Col
        xxs={12}
        xs={12}
        sm={6}
        md={6}
        lg={6}
        className="d-flex flex-column p-3 register-col"
      >
        <div className="d-flex">
          <p className="form-label">ARTIST NAME</p>
        </div>
        <MyTextInput name="name" placeholder="" disabled={disabled} />
        <Divider />
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
        <Divider />
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
        />
        <Divider />

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
      </Col>

      <Col
        xxs={12}
        xs={12}
        sm={6}
        md={6}
        lg={6}
        className="d-flex flex-column p-3 register-col"
      ></Col>
    </div>
  );
};

export default EditArtistForm;
