import { useDispatch } from "react-redux";
import { UserModel } from "../../../../app/model/dto/user.model";
import { Formik } from "formik";
import { Button, Form } from "react-bootstrap";
import TextInput from "../../../../common/form/TextInput";
import EquipmentEntry from "../../../../common/components/Equipment/EquipmentEntry";
import { useState } from "react";
import TextArea from "../../../../common/form/TextArea";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import { EquipmentModel } from "../../../../app/model/dto/equipment.model";
import MultiImageCropper from "../../../../common/components/Cropper/MultiImageCropper";
import agent from "../../../../api/agent";
import { handleResetModal } from "../../../../app/redux/actions/modalSlice";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import { handleUpdateAllowAddPackage } from "../../../../app/redux/actions/pageDataSlice";

interface Props {
  userDetails: UserModel;
  setPackages: (packages: ArtistPackageModel[]) => void;
}

const AddArtistPackage = ({ userDetails, setPackages }: Props) => {
  const [submitting, setSubmitting] = useState(false);
  const [submittingError, setSubmittingError] = useState<string | null>();
  const [equipment, setEquipment] = useState<EquipmentModel[]>([]);
  const [images, setImages] = useState<PhotoUploadModel[]>([]);
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
            onSubmit={async (values) => {
              setSubmitting(true);
              try {
                var formData = new FormData();

                var photoIds = images.map((photo) => {
                  formData.append("Photos", photo.image);
                  return photo.photoId;
                });
                formData.append("PhotoIds", JSON.stringify(photoIds));
                formData.append("PackageName", values.packageName);
                formData.append(
                  "PackageDescription",
                  values.packageDescription
                );
                formData.append("PackagePrice", values.packagePrice);
                formData.append("PackageEquipment", JSON.stringify(equipment));

                await agent.Packages.createPackage(
                  userDetails.memberId!,
                  formData
                );

                var packages = await agent.Packages.getPackages(
                  userDetails.memberId!
                );

                setPackages(packages);

                if (packages.length == 3) {
                  dispatch(handleUpdateAllowAddPackage(false));
                }

                dispatch(handleResetModal());
              } catch (err: any) {
                setSubmittingError(err);
              }
              setSubmitting(false);
            }}
          >
            {({ values, handleSubmit, isSubmitting, isValid, submitForm }) => {
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
                            <TextInput
                              name="packageName"
                              placeholder=""
                              disabled={disabled}
                            />
                          </div>

                          <EquipmentEntry
                            title={"PACKAGE EQUIPMENT"}
                            description={
                              "Add the list of equipment you will provide with this package. Type your equipment name and press enter to add it to the list."
                            }
                            equipment={equipment}
                            setEquipment={setEquipment}
                          />
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">PACKAGE DESCRIPTION</p>
                            </div>
                            <TextArea
                              name="packageDescription"
                              placeholder=""
                              rows={7}
                            />
                          </div>
                          <div className="mb-3">
                            <div className="d-flex">
                              <p className="form-label">PACKAGE PRICE PER HR</p>
                            </div>
                            <TextInput
                              name="packagePrice"
                              placeholder=""
                              disabled={disabled}
                              type="number"
                            />
                          </div>
                        </div>
                        <div className="d-flex flex-column col-12 col-md-6 px-3">
                          <div className="d-flex">
                            <p className="form-label">PACKAGE PHOTO</p>
                          </div>
                          <p className="mb-3">
                            Provide up to 3 photos of your equipment all setup
                            to help sell your package.
                          </p>
                          <MultiImageCropper
                            images={images}
                            setImages={setImages}
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                  {submittingError ? (
                    <div className="px-3 mt-3">
                      <ErrorBanner children={submittingError} />
                    </div>
                  ) : null}
                  <div className="px-3 mt-3">
                    {!submitting ? (
                      <Button
                        className={`white-button w-100 align-self-center`}
                        disabled={
                          images.length === 0 ||
                          !values.packageName ||
                          !values.packageDescription ||
                          !values.packagePrice ||
                          equipment.length === 0
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
