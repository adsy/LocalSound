import { Form, Formik } from "formik";
import editArtistRegisterValidation from "../../../../../validation/EditArtistValidation";
import InPageLoadingComponent from "../../../../../app/layout/InPageLoadingComponent";
import { useState } from "react";
import EditArtistDetailsForm from "./EditArtistDetailsForm";
import { useDispatch } from "react-redux";
import { UpdateArtistPersonalDetailsModel } from "../../../../../app/model/dto/update-artist-personal.model";
import { Button } from "react-bootstrap";
import agent from "../../../../../api/agent";
import { handleSetUserDetails } from "../../../../../app/redux/actions/userSlice";
import { UserModel } from "../../../../../app/model/dto/user.model";
import ErrorBanner from "../../../../../common/banner/ErrorBanner";
import SuccessBanner from "../../../../../common/banner/SuccessBanner";
import { handleToggleModal } from "../../../../../app/redux/actions/modalSlice";

interface Props {
  userDetails: UserModel;
}

const EditArtistDetails = ({ userDetails }: Props) => {
  const [addressError, setAddressError] = useState(false);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const [submittingUpdate, setSubmittingUpdate] = useState(false);
  const dispatch = useDispatch();

  const formValuesUntouched = (values: UpdateArtistPersonalDetailsModel) => {
    if (values.name !== userDetails.name) {
      return false;
    }
    if (values.profileUrl !== userDetails.profileUrl) {
      return false;
    }
    if (values.aboutSection !== userDetails.aboutSection) {
      return false;
    }
    if (values.address !== userDetails.address) {
      return false;
    }
    if (values.phoneNumber !== userDetails.phoneNumber) {
      return false;
    }
    if (values.spotifyUrl !== userDetails.spotifyUrl) {
      return false;
    }
    if (values.youtubeUrl !== userDetails.youtubeUrl) {
      return false;
    }
    if (values.soundcloudUrl !== userDetails.soundcloudUrl) {
      return false;
    }
    return true;
  };

  return (
    <div className="fade-in pb-4 mt-4">
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
            setShowSuccessMessage(false);
            setStatus(null);
            try {
              var submissionData = { ...values };

              await agent.Artist.updateArtistPersonalDetails(
                userDetails?.memberId!,
                submissionData
              );
              dispatch(
                handleSetUserDetails({
                  ...userDetails,
                  ...submissionData,
                })
              );

              dispatch(
                handleToggleModal({
                  open: false,
                })
              );
              setSubmittingUpdate(false);
            } catch (err) {
              setStatus({
                error:
                  "There was an error updating your details, please try again..",
              });
            }
          }}
          validationSchema={editArtistRegisterValidation}
        >
          {({
            values,
            handleSubmit,
            isSubmitting,
            isValid,
            status,
            setFieldValue,
            setFieldTouched,
            submitForm,
          }) => {
            const disabled =
              !isValid || isSubmitting || formValuesUntouched(values);
            return (
              <Form
                className="ui form error fade-in"
                onSubmit={handleSubmit}
                autoComplete="off"
              >
                <div className="form-body">
                  <EditArtistDetailsForm
                    setFieldValue={setFieldValue}
                    setFieldTouched={setFieldTouched}
                    setAddressError={setAddressError}
                    disabled={isSubmitting}
                    values={values as UpdateArtistPersonalDetailsModel}
                  />
                </div>
                {status?.error ? (
                  <ErrorBanner className="fade-in mb-0 mx-3">
                    {status.error}
                  </ErrorBanner>
                ) : null}
                {showSuccessMessage ? (
                  <SuccessBanner className="fade-in mb-0 mx-3">
                    Your personal details have been successfully updated.
                  </SuccessBanner>
                ) : null}
                <div className="px-3 mt-3">
                  {!submittingUpdate ? (
                    <Button
                      className={`black-button w-100 align-self-center`}
                      disabled={disabled || addressError}
                      onClick={() => {
                        setSubmittingUpdate(true);
                        submitForm();
                      }}
                    >
                      <h4>Update personal details</h4>
                    </Button>
                  ) : (
                    <InPageLoadingComponent />
                  )}
                </div>
              </Form>
            );
          }}
        </Formik>
      </div>
    </div>
  );
};

export default EditArtistDetails;
