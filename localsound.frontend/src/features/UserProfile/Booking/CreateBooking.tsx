import { useLayoutEffect, useState } from "react";
import { ArtistPackageModel } from "../../../app/model/dto/artist-package.model";
import { useDispatch } from "react-redux";
import { Form, Formik } from "formik";
import TextInput from "../../../common/form/TextInput";
import TextArea from "../../../common/form/TextArea";
import AddressInput from "../../../common/form/AddressInput";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import DateInput from "../../../common/form/DateInput";
import agent from "../../../api/agent";
import SelectInput from "../../../common/form/SelectInput";
import createBookingValidation from "../../../validation/CreateBookingValidation";

interface Props {
  artistPackage: ArtistPackageModel;
}

const CreateBooking = ({ artistPackage }: Props) => {
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>();
  const [addressError, setAddressError] = useState(false);
  const [eventTypes, setEventTypes] = useState<{ [key: string]: any }[]>([]);
  const dispatch = useDispatch();

  useLayoutEffect(() => {
    (async () => {
      try {
        var result = await agent.EventType.getEventTypes();
        setEventTypes(
          result.map((eventType, index) => {
            return {
              value: eventType.eventTypeId,
              text: eventType.eventTypeName,
              key: index,
            };
          })
        );
      } catch (err: any) {
        setError(err);
      }
      setLoading(false);
    })();
  }, []);

  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">Create booking</h2>
      </div>
      <div className="fade-in pb-4 mt-4">
        <div className="w-100 fade-in">
          <Formik
            initialValues={{
              bookingDescription: "",
              address: "",
              bookingLength: "",
              eventType: "",
              bookingDate: null,
            }}
            onSubmit={async (values, { setStatus }) => {
              setSubmitting(true);
              try {
                // var formData = new FormData();
                // var photoIds = images.map((photo) => {
                //   formData.append("Photos", photo.image);
                //   return photo.photoId;
                // });
                // formData.append("PhotoIds", JSON.stringify(photoIds));
                // formData.append("PackageName", values.packageName);
                // formData.append(
                //   "PackageDescription",
                //   values.packageDescription
                // );
                // formData.append("PackagePrice", values.packagePrice);
                // formData.append("PackageEquipment", JSON.stringify(equipment));
                // await agent.Packages.createPackage(
                //   userDetails.memberId!,
                //   formData
                // );
                // var packages = await agent.Packages.getPackages(
                //   userDetails.memberId!
                // );
                // setPackages(packages);
                // dispatch(handleResetModal());
              } catch (err: any) {
                setError(err);
              }
              setSubmitting(false);
            }}
            validationSchema={createBookingValidation}
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
              const disabled = !isValid || isSubmitting;
              return (
                <Form
                  className="ui form error fade-in"
                  onSubmit={handleSubmit}
                  autoComplete="off"
                >
                  <div className="form-body">
                    <div id="edit-form" className="d-flex flex-column">
                      <div className="d-flex flex-column px-3">
                        <div className="mb-3">
                          <div className="d-flex">
                            <p className="form-label">PACKAGE TYPE</p>
                          </div>
                          <h4 className="mt-1">
                            {artistPackage.artistPackageName} - $
                            {artistPackage.artistPackagePrice} per hr.
                          </h4>
                        </div>
                        <div className="d-flex flex-row justify-content-between">
                          <div className="mb-3 w-50 mr-2">
                            <div className="d-flex">
                              <p className="form-label">BOOKING DATE</p>
                            </div>
                            <DateInput
                              name="bookingDate"
                              date={values.bookingDate}
                              setFieldValue={setFieldValue}
                              setFieldTouched={setFieldTouched}
                              placeholder=""
                            />
                          </div>
                          <div className="mb-3 w-50 ml-2">
                            <div className="d-flex">
                              <p className="form-label">BOOKING LENGTH</p>
                            </div>
                            <TextInput
                              name="bookingLength"
                              placeholder=""
                              type="number"
                            />
                          </div>
                        </div>
                        {loading ? (
                          <>
                            <InPageLoadingComponent />
                          </>
                        ) : (
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">EVENT TYPE</p>
                            </div>
                            <SelectInput
                              name="eventType"
                              placeholder=""
                              options={eventTypes}
                            />
                          </div>
                        )}
                        <div className="mb-3">
                          <div className="d-flex">
                            <p className="form-label">EVENT LOCATION</p>
                          </div>
                          <p className="mt-1 mb-1">
                            Please provide the suburb of the event so the artist
                            has an idea of where they will be going.
                          </p>
                          <AddressInput
                            name="address"
                            placeholder=""
                            setFieldValue={setFieldValue}
                            setFieldTouched={setFieldTouched}
                            setAddressError={setAddressError}
                            preselectedAddress={values.address}
                          />
                        </div>
                        <div className="mb-3">
                          <div className="d-flex">
                            <p className="form-label">
                              PROVIDE MORE INFORMATION
                            </p>
                          </div>
                          <TextArea
                            name="bookingDescription"
                            placeholder=""
                            rows={4}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                  {error ? (
                    <div className="px-3 mt-3">
                      <ErrorBanner children={error} />
                    </div>
                  ) : null}
                  <div className="px-3 mt-3">
                    {!submitting ? (
                      <Button
                        className={`black-button w-100 align-self-center`}
                        disabled={disabled}
                        onClick={() => submitForm()}
                      >
                        <h4>Send</h4>
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
    </div>
  );
};

export default CreateBooking;
