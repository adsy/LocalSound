import { Form, Formik } from "formik";
import editArtistRegisterValidation from "./../../../validation/EditArtistValidation";
import { Button, Header } from "semantic-ui-react";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useState } from "react";
import EditArtistForm from "./EditArtistForm";
import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { EditArtistModel } from "../../../app/model/dto/edit-artist.model";

const EditArtistProfile = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [addressError, setAddressError] = useState(false);

  return (
    <div id="auth-modal" className="fade-in">
      <div className="d-flex flex-row header">
        <h2 className="header-title mt-1 align-self-center">
          Edit your artist profile
        </h2>
      </div>
      <div className="w-100 header mt-2 fade-in">
        <Formik
          initialValues={{
            name: userDetails?.name,
            address: userDetails?.address,
            phoneNumber: userDetails?.phoneNumber,
            soundcloudUrl: userDetails?.soundcloudUrl,
            spotifyUrl: userDetails?.spotifyUrl,
            youtubeUrl: userDetails?.youtubeUrl,
          }}
          onSubmit={async (values, { setStatus }) => {
            setStatus(null);
            // await handleRegisterRequest(
            //   values,
            //   CustomerTypes.Artist,
            //   setStatus
            // );
          }}
          validationSchema={editArtistRegisterValidation}
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
                  <EditArtistForm
                    setFieldValue={setFieldValue}
                    setFieldTouched={setFieldTouched}
                    setAddressError={setAddressError}
                    disabled={isSubmitting}
                    values={values as EditArtistModel}
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
                    <strong>UPDATE PROFILE</strong>
                  </Button>
                ) : (
                  <InPageLoadingComponent />
                )}
              </Form>
            );
          }}
        </Formik>
      </div>
    </div>
  );
};

export default EditArtistProfile;
