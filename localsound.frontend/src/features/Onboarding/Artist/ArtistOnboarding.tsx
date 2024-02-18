import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { EquipmentModel } from "../../../app/model/dto/equipment.model";
import { GenreModel } from "../../../app/model/dto/genre.model";
import { EventTypeModel } from "../../../app/model/dto/eventType.model";
import { Form, Formik } from "formik";
import { handleSaveProfileData } from "../../../app/redux/actions/userSlice";
import SearchGenreTypes from "../../../common/components/Search/SearchGenreTypes";
import SearchEventTypes from "../../../common/components/Search/SearchEventTypes";
import EquipmentEntry from "../../../common/components/Equipment/EquipmentEntry";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import SuccessBanner from "../../../common/banner/SuccessBanner";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import agent from "../../../api/agent";
import UpdateProfilePhoto from "../../../common/components/Photo/UpdateProfilePhoto";
import TextArea from "../../../common/form/TextArea";

const ArtistOnboarding = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails)!;
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
  const [equipment, setEquipment] = useState<EquipmentModel[]>([]);
  const [eventTypes, setEventTypes] = useState<EventTypeModel[]>([]);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const [onboardingError, setOnboardingError] = useState<string | null>(null);
  const dispatch = useDispatch();

  useEffect(() => {
    setSelectedGenres([...userDetails?.genres]);
    setEquipment([...userDetails?.equipment]);
    setEventTypes([...userDetails?.eventTypes]);
  }, [userDetails]);

  const formValuesUntouched = (values: { [key: string]: string }) => {
    if (
      JSON.stringify([...userDetails.genres]) !==
      JSON.stringify([...selectedGenres])
    ) {
      return false;
    }
    if (
      JSON.stringify([...userDetails.eventTypes]) !==
      JSON.stringify([...eventTypes])
    ) {
      return false;
    }
    if (
      JSON.stringify([...userDetails.equipment]) !==
      JSON.stringify([...equipment])
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
          initialValues={{
            about: userDetails.aboutSection ? userDetails.aboutSection : "",
          }}
          onSubmit={async (values) => {
            setOnboardingError(null);
            if (showSuccessMessage) {
              setShowSuccessMessage(false);
            }

            try {
              var submissionData = {
                aboutSection: values.about,
                genres: [...selectedGenres],
                equipment: [...equipment],
                eventTypes: [...eventTypes],
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
            } catch (err: any) {
              setOnboardingError(err);
            }
          }}
        >
          {({ handleSubmit, isSubmitting, isValid, submitForm, values }) => {
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
                  <div className="d-flex flex-column col-12 px-3 mb-3">
                    <div className="d-flex mb-1">
                      <p className="form-label">ABOUT</p>
                    </div>
                    <p className="text-justify mb-0">
                      Lets get to know you. Add a little bit about yourself here
                      which will appear on your profile!
                    </p>
                    <TextArea name="about" placeholder="" rows={4} />
                  </div>
                  <div className="d-flex flex-row flex-wrap justify-content-between">
                    <div className="d-flex flex-column col-12 col-md-6 px-3">
                      <SearchGenreTypes
                        title={"GENRES"}
                        description={
                          "Start typing a genre you play to attach to your profile. Attaching genres to your profile allow people to find you specifically for their next event."
                        }
                        selectedGenres={selectedGenres}
                        setSelectedGenres={setSelectedGenres}
                        placeholder={"Add up to 10 genres for your profile"}
                      />
                    </div>
                    <div className="d-flex flex-column col-12 col-md-6 px-3">
                      <SearchEventTypes
                        title={"EVENT TYPES"}
                        description={
                          "Search for event types which you would be available for. Attaching event types to your profile allow people to find you for the event type they want to host."
                        }
                        eventTypes={eventTypes}
                        setEventTypes={setEventTypes}
                      />
                    </div>
                  </div>
                  <div className="px-3">
                    <EquipmentEntry
                      title={"PARTY EQUIPMENT"}
                      description={
                        "Everyone has different a different party setup. Show how prepared you are for your next party you play at. Type the name of your equipment and press enter to add it to your list."
                      }
                      equipment={equipment}
                      setEquipment={setEquipment}
                    />
                  </div>
                </div>
                {onboardingError ? (
                  <ErrorBanner className="fade-in mb-0 mx-3">
                    {onboardingError}
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
                      className={`white-button w-100 align-self-center`}
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

export default ArtistOnboarding;
