import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { EquipmentModel } from "../../../app/model/dto/equipment.model";
import { GenreModel } from "../../../app/model/dto/genre.model";
import { EventTypeModel } from "../../../app/model/dto/eventType.model";
import { Form, Formik } from "formik";
import { handleSaveProfileData } from "../../../app/redux/actions/userSlice";
import SearchGenreTypes from "../../../common/components/Search/SearchGenreTypes";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import SuccessBanner from "../../../common/banner/SuccessBanner";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import agent from "../../../api/agent";
import UpdateProfilePhoto from "../../../common/components/Photo/UpdateProfilePhoto";
import TextArea from "../../../common/form/TextArea";

const NonArtistOnboarding = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails)!;
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const dispatch = useDispatch();

  useEffect(() => {
    setSelectedGenres([...userDetails?.genres]);
  }, [userDetails]);

  const formValuesUntouched = (values: { [key: string]: string }) => {
    if (
      JSON.stringify([...userDetails.genres]) !==
      JSON.stringify([...selectedGenres])
    ) {
      return false;
    }
    if (values.aboutSection !== userDetails.aboutSection) {
      return false;
    }
    return true;
  };

  return (
    <div id="modal-popup" className="fade-in">
      <div className="d-flex flex-row">
        <h2 className="header-title align-self-center">Lets get you setup!</h2>
      </div>
      <div className="w-100 fade-in mt-3">
        <Formik
          initialValues={{}}
          onSubmit={async (values, { setStatus }) => {
            setStatus(null);
            if (showSuccessMessage) {
              setShowSuccessMessage(false);
            }

            try {
              var submissionData = {
                genres: [...selectedGenres],
              };

              await agent.Account.saveOnboardingData(
                userDetails?.memberId!,
                submissionData
              );

              dispatch(
                handleSaveProfileData({
                  ...userDetails,
                  ...submissionData,
                })
              );

              setShowSuccessMessage(true);
            } catch (err) {
              setStatus({
                error:
                  "There was an error saving your details, please try again..",
              });
            }
          }}
        >
          {({
            handleSubmit,
            isSubmitting,
            isValid,
            status,
            submitForm,
            values,
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
                  <UpdateProfilePhoto />
                  <div className="d-flex flex-row flex-wrap justify-content-between">
                    <div className="d-flex flex-column col-12 px-3">
                      <SearchGenreTypes
                        title={"GENRES"}
                        description={
                          "Start typing a genre you play to attach to your profile. Attaching genres to your profile will allow us to find local artists who play the music you're into!"
                        }
                        selectedGenres={selectedGenres}
                        setSelectedGenres={setSelectedGenres}
                        placeholder={"Add up to 10 genres for your profile"}
                      />
                    </div>
                  </div>
                </div>
                {status?.error ? (
                  <ErrorBanner className="fade-in mb-0 mx-3">
                    {status.error}
                  </ErrorBanner>
                ) : null}
                {showSuccessMessage ? (
                  <SuccessBanner className="fade-in mb-0 mx-3">
                    Your profile details have been successfully updated.
                  </SuccessBanner>
                ) : null}
                <div className="px-3 mt-3">
                  {!isSubmitting ? (
                    <Button
                      className={`black-button w-100 align-self-center`}
                      disabled={disabled}
                      onClick={() => submitForm()}
                    >
                      <h4>Save details</h4>
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

export default NonArtistOnboarding;
