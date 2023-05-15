import { Formik } from "formik";
import { Button } from "react-bootstrap";
import { Divider, Form, Header } from "semantic-ui-react";
import nonArtistValidation from "../../../validation/NonArtistValidation";
import { useDispatch } from "react-redux";
import { useState } from "react";
import NonArtistRegisterForm from "./NonArtistRegisterForm";
import {
  RegistrationModel,
  UserRegistrationModel,
} from "../../../app/model/dto/user-registration.model";
import agent from "../../../api/agent";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";

const NonArtistRegister = () => {
  const dispatch = useDispatch();
  const [addressError, setAddressError] = useState(false);

  const handleRegisterRequest = async (
    values: RegistrationModel,
    setStatus: (value: any) => void
  ) => {
    try {
      var result = await agent.Authentication.register({
        customerType: CustomerTypes.NonArtist,
        registrationDto: values,
      });

      //TODO: Do something with result
      console.log(result);
    } catch (error) {
      if (error) {
        setStatus(error);
      } else {
        setStatus({
          error:
            "An error occured while trying to register your account, please try again..",
        });
      }
    }
  };

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
        await handleRegisterRequest(values, setStatus);
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
      }) => {
        const disabled = !isValid || !dirty || isSubmitting;
        return (
          <Form
            className="ui form error fade-in"
            onSubmit={handleSubmit}
            autoComplete="off"
          >
            <div className="form-body">
              <NonArtistRegisterForm
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
            {
              !isSubmitting ? (
                <Button
                  className={`purple-button w-100 align-self-center ${
                    status?.error ? "mt-3" : "mt-4"
                  }`}
                  disabled={disabled || addressError}
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
