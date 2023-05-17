import { Button, Col } from "react-bootstrap";
import { RegistrationModel } from "../../../app/model/dto/user-registration.model";
import PasswordStrengthBar from "react-password-strength-bar";
import { Divider } from "semantic-ui-react";
import MyAddressInput from "../../../common/form/MyAddressInput";
import MyTextInput from "../../../common/form/MyTextInput";
import { useState } from "react";

interface Props {
  disabled?: boolean;
  password: string;
  setFieldValue: (field: string, value: any, shouldValidate?: boolean) => void;
  setFieldTouched: (
    field: string,
    isTouched?: boolean,
    shouldValidate?: boolean
  ) => void;
  setAddressError: (addressError: boolean) => void;
  values: RegistrationModel;
}

const ArtistRegisterForm = (props: Props) => {
  const {
    password,
    disabled,
    setFieldValue,
    setFieldTouched,
    setAddressError,
    values,
  } = props;

  const [showSoundcloudInput, setShowSoundcloudInput] = useState(false);
  const [showSpotifyInput, setShowSpotifyInput] = useState(false);

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
        <div className="mb-2">
          Your mobile number will be used for account verification and other
          methods of contact.
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
        <Divider />
        <div className="mb-2">
          Your address can be used to help locate performers that are closest to
          you.
        </div>
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
        <div className="mb-2">
          Adding your social media profiles is optional.
        </div>
        {showSoundcloudInput ? (
          <>
            <div className="d-flex">
              <p className="form-label">SOUNDCLOUD URL</p>
            </div>
            <MyTextInput name="soundcloudUrl" placeholder="" />
          </>
        ) : (
          <>
            <Button
              onClick={() => setShowSoundcloudInput(!showSoundcloudInput)}
              className="purple-button w-fit-content d-flex flex-row mb-3"
            >
              <div className="facebook-icon-nav align-self-center mr-2"></div>
              <h4 className="align-self-center mt-0">Add Soundcloud</h4>
            </Button>
          </>
        )}
        {showSpotifyInput ? (
          <>
            <div className="d-flex">
              <p className="form-label">SPOTIFY URL</p>
            </div>
            <MyTextInput name="spotifyUrl" placeholder="" />
          </>
        ) : (
          <>
            <Button
              onClick={() => setShowSpotifyInput(!showSpotifyInput)}
              className="purple-button w-fit-content d-flex flex-row mb-3 "
            >
              <div className="instagram-icon-nav align-self-center mr-2"></div>
              <h4 className="align-self-center mt-0">Add Spotify</h4>
            </Button>
          </>
        )}
      </Col>

      <Col
        xxs={12}
        xs={12}
        sm={6}
        md={6}
        lg={6}
        className="d-flex flex-column p-3 register-col"
      >
        <div className="d-flex">
          <p className="form-label">EMAIL</p>
        </div>
        <MyTextInput
          name="email"
          placeholder=""
          className=""
          disabled={disabled}
        />
        <div className="d-flex">
          <p className="form-label">PASSWORD</p>
        </div>
        <MyTextInput
          name="password"
          placeholder=""
          type="password"
          disabled={disabled}
        />
        <PasswordStrengthBar minLength={6} password={password} />

        <div className="d-flex">
          <p className="form-label">RE-ENTER PASSWORD</p>
        </div>
        <MyTextInput
          name="checkPassword"
          placeholder=""
          type="password"
          disabled={disabled}
        />
      </Col>
    </div>
  );
};

export default ArtistRegisterForm;
