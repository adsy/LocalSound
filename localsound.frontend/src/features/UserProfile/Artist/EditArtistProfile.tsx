import { Form, Formik } from "formik";
import editArtistRegisterValidation from "./../../../validation/EditArtistValidation";
import { Header } from "semantic-ui-react";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useState } from "react";
import EditArtistForm from "./EditArtistForm";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { UpdateArtistModel } from "../../../app/model/dto/update-artist";
import { Button } from "react-bootstrap";
import agent from "../../../api/agent";
import { handleSetUserDetails } from "../../../app/redux/actions/userSlice";
import { handleResetModal } from "../../../app/redux/actions/modalSlice";

const EditArtistProfile = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [addressError, setAddressError] = useState(false);
  const dispatch = useDispatch();

  return (
    <div id="auth-modal" className="fade-in">
      <div className="d-flex flex-row header">
        <h2 className="header-title mt-1 align-self-center">
          Edit your artist profile
        </h2>
      </div>
      <div className="w-100 fade-in">
        <Formik
          initialValues={{
            name: userDetails?.name,
            address: userDetails?.address!,
            phoneNumber: userDetails?.phoneNumber!,
            soundcloudUrl: userDetails?.soundcloudUrl,
            spotifyUrl: userDetails?.spotifyUrl,
            youtubeUrl: userDetails?.youtubeUrl,
            aboutSection: userDetails?.aboutSection,
            profileUrl: userDetails?.profileUrl,
          }}
          onSubmit={async (values, { setStatus }) => {
            setStatus(null);
            try {
              await agent.Artist.updateArtistDetails(
                userDetails?.memberId!,
                values
              );

              dispatch(
                handleSetUserDetails({
                  ...userDetails,
                  ...values,
                })
              );
              dispatch(handleResetModal());
            } catch (err) {
              //TODO: Do something with the errors
              console.log(err);
            }
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
            errors,
          }) => {
            const disabled = !isValid || isSubmitting;
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
                    values={values as UpdateArtistModel}
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
                    <h4>Update profile</h4>
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
