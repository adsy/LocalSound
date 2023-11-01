import { useEffect, useState } from "react";
import { GenreModel } from "../../../../../app/model/dto/genre.model";
import { UserModel } from "../../../../../app/model/dto/user.model";
import { Button, Form } from "react-bootstrap";
import { Formik } from "formik";
import { useDispatch } from "react-redux";
import agent from "../../../../../api/agent";
import { handleSetUserDetails } from "../../../../../app/redux/actions/userSlice";
import ErrorBanner from "../../../../../common/banner/ErrorBanner";
import InPageLoadingComponent from "../../../../../app/layout/InPageLoadingComponent";
import { EquipmentModel } from "../../../../../app/model/dto/equipment.model";
import { EventTypeModel } from "../../../../../app/model/dto/eventType.model";
import SearchEventTypes from "../Search/SearchEventTypes";
import SuccessBanner from "../../../../../common/banner/SuccessBanner";
import EquipmentEntry from "./EquipmentEntry";
import SearchGenreTypes from "../Search/SearchGenreTypes";

interface Props {
  userDetails: UserModel;
}

const EditArtistProfile = ({ userDetails }: Props) => {
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
  const [equipment, setEquipment] = useState<EquipmentModel[]>([]);
  const [eventTypes, setEventTypes] = useState<EventTypeModel[]>([]);
  const [showSuccessMessage, setShowSuccessMessage] = useState(false);
  const dispatch = useDispatch();

  useEffect(() => {
    setSelectedGenres([...userDetails?.genres]);
    setEquipment([...userDetails?.equipment]);
    setEventTypes([...userDetails?.eventTypes]);
  }, [userDetails]);

  const formValuesUntouched = () => {
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
    return true;
  };

  return (
    <div className="fade-in pb-4 mt-4">
      <div className="w-100 fade-in">
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
                equipment: [...equipment],
                eventTypes: [...eventTypes],
              };

              await agent.Artist.updateArtistProfileDetails(
                userDetails?.memberId!,
                submissionData
              );

              dispatch(
                handleSetUserDetails({
                  ...userDetails,
                  ...submissionData,
                })
              );

              setShowSuccessMessage(true);
            } catch (err) {
              setStatus({
                error:
                  "There was an error updating your details, please try again..",
              });
            }
          }}
        >
          {({ handleSubmit, isSubmitting, isValid, status, submitForm }) => {
            const disabled = !isValid || isSubmitting || formValuesUntouched();
            return (
              <Form
                className="ui form error fade-in"
                onSubmit={handleSubmit}
                autoComplete="off"
              >
                <div className="form-body">
                  <div className="d-flex flex-row flex-wrap justify-content-between">
                    <div className="d-flex flex-column col-12 col-md-6 px-3">
                      <div className="mb-3">
                        <div className="d-flex mb-1">
                          <p className="form-label">GENRES</p>
                        </div>
                        <p className="text-justify">
                          Start typing a genre you play to attach to your
                          profile. Attaching genres to your profile allow people
                          to find you specifically for their next event.
                        </p>
                        <SearchGenreTypes
                          selectedGenres={selectedGenres}
                          setSelectedGenres={setSelectedGenres}
                          placeholder={"Add up to 10 genres for your profile"}
                        />
                      </div>
                    </div>
                    <div className="d-flex flex-column col-12 col-md-6 px-3">
                      <div className="mb-3">
                        <div className="d-flex mb-1">
                          <p className="form-label">EVENT TYPES</p>
                        </div>
                        <p>
                          Search for event types which you would be available
                          for. Attaching event types to your profile allow
                          people to find you for the event type they want to
                          host.
                        </p>
                        <SearchEventTypes
                          eventTypes={eventTypes}
                          setEventTypes={setEventTypes}
                        />
                      </div>
                    </div>
                  </div>
                  <div className="d-flex flex-row px-3">
                    <div className="mb-3">
                      <div className="d-flex mb-1">
                        <p className="form-label">PARTY EQUIPMENT</p>
                      </div>
                      <p className="text-justify">
                        Everyone has different a different party setup. Show how
                        prepared you are for your next party you play at. Type
                        the name of your equipment and press enter to add it to
                        your list.
                      </p>
                      <EquipmentEntry
                        equipment={equipment}
                        setEquipment={setEquipment}
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
                      <h4>Update profile details</h4>
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

export default EditArtistProfile;
