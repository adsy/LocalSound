import { Form, Formik } from "formik";
import editArtistRegisterValidation from "../../../../../validation/EditArtistValidation";
import { Header } from "semantic-ui-react";
import InPageLoadingComponent from "../../../../../app/layout/InPageLoadingComponent";
import { useState } from "react";
import EditArtistDetailsForm from "./EditArtistDetailsForm";
import { useDispatch } from "react-redux";
import { UpdateArtistModel } from "../../../../../app/model/dto/update-artist.model";
import { Button } from "react-bootstrap";
import agent from "../../../../../api/agent";
import { handleSetUserDetails } from "../../../../../app/redux/actions/userSlice";
import { handleResetModal } from "../../../../../app/redux/actions/modalSlice";
import { UserModel } from "../../../../../app/model/dto/user.model";

interface Props {
  userDetails: UserModel;
}

const EditArtistDetails = ({ userDetails }: Props) => {
  const [addressError, setAddressError] = useState(false);
  const dispatch = useDispatch();

  return (
    <div className="fade-in pb-4 mt-5">
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
                  <EditArtistDetailsForm
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
                <div className="px-3 mt-4">
                  {!isSubmitting ? (
                    <Button
                      className={`black-button w-100 px-5 align-self-center ${
                        status?.error ? "mt-3" : "mt-4"
                      }`}
                      disabled={disabled || addressError}
                    >
                      <h4>Update details</h4>
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
