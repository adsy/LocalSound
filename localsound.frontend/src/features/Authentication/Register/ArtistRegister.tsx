import { Formik } from "formik";
import { Button } from "react-bootstrap";
import { Form, Header } from "semantic-ui-react";
import { useState } from "react";
import NonArtistRegisterForm from "./NonArtistRegisterForm";
import { RegistrationModel } from "../../../app/model/dto/user-registration.model";
import artistRegisterValidation from "../../../validation/ArtistRegisterValidation";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import ArtistRegisterForm from "./ArtistRegisterForm";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";

interface Props {
  handleRegisterRequest: (
    values: RegistrationModel,
    customerType: CustomerTypes,
    setStatus: (value: any) => void
  ) => void;
}

const ArtistRegister = ({ handleRegisterRequest }: Props) => {
  const [addressError, setAddressError] = useState(false);

  return (
    <Formik
      initialValues={{
        profileUrl: "",
        email: "",
        password: "",
        checkPassword: "",
        name: "",
        address: "",
        phoneNumber: "",
        soundcloudUrl: "",
        spotifyUrl: "",
        youtubeUrl: "",
      }}
      onSubmit={async (values, { setStatus }) => {
        setStatus(null);
        await handleRegisterRequest(values, CustomerTypes.Artist, setStatus);
      }}
      validationSchema={artistRegisterValidation}
    >
      {({
        values,
        handleSubmit,
        isSubmitting,
        isValid,
        dirty,
        status,
        setFieldValue,
        setFieldTouched,
      }) => {
        const disabled = !isValid || !dirty || isSubmitting;
        return (
          <Form
            className="ui form error fade-in"
            onSubmit={handleSubmit}
            autoComplete="off"
          >
            <div className="form-body">
              <ArtistRegisterForm
                password={values.password}
                setFieldValue={setFieldValue}
                setFieldTouched={setFieldTouched}
                setAddressError={setAddressError}
                disabled={isSubmitting}
                values={values as RegistrationModel}
              />
            </div>
            {status?.error ? (
              <Header
                color="black"
                as="h4"
                content={status.error}
                className="text-center fade-in mb-3"
              />
            ) : null}
            {!isSubmitting ? (
              <Button
                className={`black-button w-100 align-self-center ${
                  status?.error ? "mt-3" : "mt-4"
                }`}
                disabled={disabled || addressError}
                type="submit"
              >
                <strong>REGISTER</strong>
              </Button>
            ) : (
              <InPageLoadingComponent />
            )}
          </Form>
        );
      }}
    </Formik>
  );
};

export default ArtistRegister;
