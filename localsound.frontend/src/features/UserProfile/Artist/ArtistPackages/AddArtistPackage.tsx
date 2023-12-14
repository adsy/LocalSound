import { useDispatch } from "react-redux";
import { UserModel } from "../../../../app/model/dto/user.model";
import { Formik } from "formik";
import { Button, Form } from "react-bootstrap";
import MyTextInput from "../../../../common/form/MyTextInput";
import EquipmentEntry from "../Edit/EditArtistProfile/EquipmentEntry";
import { useState } from "react";
import MyTextArea from "../../../../common/form/MyTextArea";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import { EquipmentModel } from "../../../../app/model/dto/equipment.model";

interface Props {
  userDetails: UserModel;
}

const AddArtistPackage = ({ userDetails }: Props) => {
  const [equipment, setEquipment] = useState<EquipmentModel[]>([]);
  const dispatch = useDispatch();

  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">Create package</h2>
      </div>
      <div className="fade-in pb-4 mt-4">
        <div className="w-100 fade-in">
          <Formik
            initialValues={{
              packageName: "",
              packageDescription: "",
              packagePrice: "",
            }}
            onSubmit={async (values, { setStatus }) => {}}
          >
            {({
              values,
              handleSubmit,
              isSubmitting,
              isValid,
              status,
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
                      <div className="d-flex flex-row flex-wrap justify-content-between">
                        <div className="d-flex flex-column col-12 col-md-6 px-3">
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">PACKAGE NAME</p>
                            </div>
                            <MyTextInput
                              name="trackName"
                              placeholder=""
                              disabled={disabled}
                            />
                          </div>
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">PACKAGE EQUIPMENT</p>
                            </div>
                            <p className="text-justify">
                              Add the list of equipment you will provide with
                              this package.
                            </p>
                            <EquipmentEntry
                              equipment={equipment}
                              setEquipment={setEquipment}
                            />
                          </div>
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">PACKAGE DESCRIPTION</p>
                            </div>
                            <MyTextArea
                              name="packageDescription"
                              placeholder=""
                              rows={7}
                            />
                          </div>
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">PACKAGE PRICE PER HR</p>
                            </div>
                            <MyTextInput
                              name="packagePrice"
                              placeholder=""
                              disabled={disabled}
                            />
                          </div>
                        </div>
                        <div className="d-flex flex-column col-12 col-md-6 px-3">
                          <div className="d-flex">
                            <p className="form-label">PACKAGE PHOTO</p>
                          </div>
                          <p className="mb-3">
                            Provide some photos of your equipment all setup to
                            help sell your package.
                          </p>
                          {/* {!updatingTrackPhoto ? (
                            <>
                              <Image
                                src={getDisplayImage()}
                                size="medium"
                                className="align-self-center mb-2"
                                style={{ borderRadius: "5px" }}
                              />

                              <label
                                htmlFor="trackUpload"
                                className="btn white-button fade-in-out w-fit-content align-self-center px-5"
                              >
                                <h4>Select photo</h4>
                              </label>
                              <input
                                type="file"
                                accept=".jpg,.png,.jpeg"
                                id="trackUpload"
                                style={{ display: "none" }}
                                onChange={async (event) => {
                                  if (event?.target?.files) {
                                    setTrackImage(event.target.files[0]);
                                    setUpdatingTrackPhoto(true);
                                  }
                                }}
                              />
                            </>
                          ) : trackImage ? (
                            <ImageCropper
                              file={trackImage}
                              onFileUpload={onFileUpload}
                              cancelCrop={cancelCrop}
                              cropType={CropTypes.Square}
                            />
                          ) : null} */}
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="px-3 mt-3">
                    {!isSubmitting ? (
                      <Button
                        className={`black-button w-100 align-self-center`}
                        disabled={
                          disabled ||
                          // uploadDataError ||
                          // !file ||
                          !values.packageName ||
                          !values.packageDescription ||
                          !values.packagePrice ||
                          equipment.length == 0
                        }
                        onClick={() => submitForm()}
                      >
                        <h4>Add package</h4>
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

export default AddArtistPackage;
