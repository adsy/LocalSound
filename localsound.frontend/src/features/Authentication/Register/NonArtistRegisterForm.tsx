import { Col } from "react-bootstrap";
import { RegistrationModel } from "../../../app/model/dto/user-registration.model";
import PasswordStrengthBar from "react-password-strength-bar";
import { Divider } from "semantic-ui-react";
import AddressInput from "../../../common/form/AddressInput";
import TextInput from "../../../common/form/TextInput";

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

const NonArtistRegisterForm = (props: Props) => {
  const {
    password,
    disabled,
    setFieldValue,
    setFieldTouched,
    setAddressError,
    values,
  } = props;

  const handleMobileNumberChange = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.FocusEvent<HTMLInputElement, Element>
  ): void => {
    setFieldTouched("phoneNumber", true);
    let value = e.target.value;

    value = value.trim();
    setFieldValue("phoneNumber", value);
    if (value.length > 10) {
      setFieldValue("phoneNumber", value.slice(0, 12));
    }
  };

  return (
    <div className="d-flex flex-row flex-wrap p-2 pb-0 register-form">
      <Col
        xxs={12}
        xs={12}
        sm={6}
        md={6}
        lg={6}
        className="d-flex flex-column p-3 register-col"
      >
        <div className="d-flex">
          <p className="form-label">FIRST NAME</p>
        </div>
        <TextInput
          name="firstName"
          placeholder=""
          disabled={disabled}
          className="mb-2"
        />
        <div className="d-flex">
          <p className="form-label">SURNAME</p>
        </div>
        <TextInput
          name="lastName"
          placeholder=""
          disabled={disabled}
          className="mb-2"
        />
        <Divider />
        <div className="mb-2">
          Your address can be used to help locate performers that are closest to
          you.
        </div>
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
        />
        <Divider />
        <div className="mb-2">
          Your mobile number will be used for account verification and other
          methods of contact.
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
      </Col>

      <Col
        xxs={12}
        xs={12}
        sm={6}
        md={6}
        lg={6}
        className="d-flex flex-column p-3 register-col"
      >
        <div className="mb-2">
          You can set your own profile url so you can share it with others and
          be found easier.
        </div>
        <div className="d-flex">
          <p className="form-label">PROFILE URL</p>
        </div>
        <TextInput
          name="profileUrl"
          placeholder=""
          className=""
          disabled={disabled}
        />
        <Divider />
        <div className="d-flex">
          <p className="form-label">EMAIL</p>
        </div>
        <TextInput
          name="email"
          placeholder=""
          className="mb-2"
          disabled={disabled}
        />
        <div className="d-flex">
          <p className="form-label">PASSWORD</p>
        </div>
        <TextInput
          name="password"
          placeholder=""
          type="password"
          className="mb-2"
          disabled={disabled}
        />
        <PasswordStrengthBar password={password} />

        <div className="d-flex">
          <p className="form-label">RE-ENTER PASSWORD</p>
        </div>
        <TextInput
          name="checkPassword"
          placeholder=""
          type="password"
          className="mb-2"
          disabled={disabled}
        />
      </Col>
    </div>
  );
};

export default NonArtistRegisterForm;
