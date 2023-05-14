import { Formik } from "formik";
import { Button } from "react-bootstrap";
import { Divider, Form, Header } from "semantic-ui-react";
import nonArtistValidation from "../../../validation/NonArtistValidation";
import { useDispatch } from "react-redux";
import { useState } from "react";
import NonArtistRegisterForm from "./NonArtistRegisterForm";
import { UserRegistrationModel } from "../../../app/model/dto/user-registration.model";

const NonArtistRegister = () => {
  const dispatch = useDispatch();
  const [addressError, setAddressError] = useState(false);

  return (
    <Formik
      initialValues={{
        email: "",
        password: "",
        checkPassword: "",
        firstName: "",
        lastName: "",
        address: "",
        phoneNumber: "",
      }}
      onSubmit={async (values, { setErrors, setStatus }) => {
        setStatus(null);
        // await handleRegisterRequest(values, setStatus);
      }}
      validationSchema={nonArtistValidation}
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
        setFieldError,
        errors,
      }) => {
        const disabled = !isValid || !dirty || isSubmitting;
        return (
          <Form
            className="ui form error fade-in"
            onSubmit={handleSubmit}
            autoComplete="off"
          >
            <NonArtistRegisterForm
              password={values.password}
              setFieldValue={setFieldValue}
              setFieldTouched={setFieldTouched}
              setAddressError={setAddressError}
              disabled={isSubmitting}
              values={values as UserRegistrationModel}
            />
            {status?.error ? (
              <Header
                color="black"
                as="h4"
                content={status.error}
                className="text-center fade-in mb-3"
              />
            ) : null}
            <Divider />
            {
              !isSubmitting ? (
                <Button
                  className="purple-button w-100 align-self-center fade-in"
                  //   disabled={disabled || addressError}
                  disabled={disabled}
                  type="submit"
                >
                  <strong>REGISTER</strong>
                </Button>
              ) : null
              //   <InPageLoadingComponent />
            }
          </Form>
        );
      }}
    </Formik>
  );
};

export default NonArtistRegister;
