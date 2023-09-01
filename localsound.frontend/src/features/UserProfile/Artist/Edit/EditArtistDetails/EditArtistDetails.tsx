import { Form, Formik } from "formik";
import editArtistRegisterValidation from "../../../../../validation/EditArtistValidation";
import { Header } from "semantic-ui-react";
import InPageLoadingComponent from "../../../../../app/layout/InPageLoadingComponent";
import { useEffect, useState } from "react";
import EditArtistDetailsForm from "./EditArtistDetailsForm";
import { useDispatch } from "react-redux";
import { UpdateArtistModel } from "../../../../../app/model/dto/update-artist.model";
import { Button } from "react-bootstrap";
import agent from "../../../../../api/agent";
import { handleSetUserDetails } from "../../../../../app/redux/actions/userSlice";
import { handleResetModal } from "../../../../../app/redux/actions/modalSlice";
import { UserModel } from "../../../../../app/model/dto/user.model";
import { GenreModel } from "../../../../../app/model/dto/genre.model";

interface Props {
  userDetails: UserModel;
}

const EditArtistDetails = ({ userDetails }: Props) => {
  const [addressError, setAddressError] = useState(false);
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
  const dispatch = useDispatch();

  useEffect(() => {
    setSelectedGenres([...userDetails.genres]);
  }, [userDetails]);

  const formValuesUntouched = (values: UpdateArtistModel) => {
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
    if (
      JSON.stringify([...userDetails.genres]) !==
      JSON.stringify([...selectedGenres])
    ) {
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
            setStatus(null);
            try {
              var submissionData = { ...values, genres: selectedGenres };

              await agent.Artist.updateArtistDetails(
                userDetails?.memberId!,
                submissionData
              );

              dispatch(
                handleSetUserDetails({
                  ...userDetails,
                  ...submissionData,
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
            submitForm,
            touched,
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
                    values={values as UpdateArtistModel}
                    selectedGenres={selectedGenres}
                    setSelectedGenres={setSelectedGenres}
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
                      onClick={() => submitForm()}
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
