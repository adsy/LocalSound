import { useLayoutEffect, useState } from "react";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import { UserModel } from "../../../../app/model/dto/user.model";
import { EquipmentModel } from "../../../../app/model/dto/equipment.model";
import { useDispatch } from "react-redux";
import { Form, Formik } from "formik";
import { handleResetModal } from "../../../../app/redux/actions/modalSlice";
import agent from "../../../../api/agent";
import TextInput from "../../../../common/form/TextInput";
import EquipmentEntry from "../Edit/EditArtistProfile/EquipmentEntry";
import TextArea from "../../../../common/form/TextArea";
import MultiImageCropper from "../../../../common/components/MultiImageCropper/MultiImageCropper";
import { Button } from "react-bootstrap";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import ErrorBanner from "../../../../common/banner/ErrorBanner";

interface Props {
  artistPackage: ArtistPackageModel;
  artistDetails: UserModel;
  setPackages: (packages: ArtistPackageModel[]) => void;
}

const EditArtistPackage = ({
  artistPackage,
  artistDetails,
  setPackages,
}: Props) => {
  const [submitting, setSubmitting] = useState(false);
  const [submittingError, setSubmittingError] = useState<string | null>();
  const [loadingForm, setLoadingForm] = useState(true);
  const [equipment, setEquipment] = useState<EquipmentModel[]>([]);
  const [images, setImages] = useState<PhotoUploadModel[]>([]);
  const dispatch = useDispatch();

  useLayoutEffect(() => {
    (async () => {
      var photos = await Promise.all(
        artistPackage.photos.map(async (photo) => {
          var photoBlob = await fetch(photo.artistPackagePhotoUrl).then(
            (response) => response.blob()
          );
          return {
            photoId: photo.artistPackagePhotoId,
            image: photoBlob,
          } as PhotoUploadModel;
        })
      );

      setImages(photos ?? []);
      setEquipment(artistPackage.equipment);
      setLoadingForm(false);
    })();
  }, []);

  return (
    <div id="modal-popup">
      <>
        <div className="d-flex flex-row mb-4">
          <h2 className="header-title">Update package</h2>
        </div>
        <div className="fade-in pb-4 mt-4">
          {loadingForm ? (
            <div className="mt-4">
              <InPageLoadingComponent />
            </div>
          ) : (
            <div className="w-100 fade-in">
              <Formik
                initialValues={{
                  packageName: artistPackage.artistPackageName,
                  packageDescription: artistPackage.artistPackageDescription,
                  packagePrice: artistPackage.artistPackagePrice,
                }}
                onSubmit={async (values, { setStatus }) => {
                  try {
                    var formData = new FormData();

                    // get deleted photo ids
                    var deletedIds = [] as string[];
                    artistPackage.photos.forEach((photo) => {
                      var existingPhoto = images.find(
                        (x) => x.photoId == photo.artistPackagePhotoId
                      );
                      if (!existingPhoto) {
                        deletedIds.push(photo.artistPackagePhotoId);
                      }
                    });

                    formData.append(
                      "DeletedPhotoIds",
                      JSON.stringify(deletedIds)
                    );

                    var newIds = [] as string[];
                    images.forEach((photo) => {
                      var existingPhoto = artistPackage.photos.find(
                        (x) => x.artistPackagePhotoId == photo.photoId
                      );
                      if (!existingPhoto) {
                        formData.append("Photos", photo.image);
                        newIds.push(photo.photoId);
                      }
                    });

                    formData.append("PhotoIds", JSON.stringify(newIds));
                    formData.append("PackageName", values.packageName);
                    formData.append(
                      "PackageDescription",
                      values.packageDescription
                    );
                    formData.append("PackagePrice", values.packagePrice);
                    formData.append(
                      "PackageEquipment",
                      JSON.stringify(equipment)
                    );

                    await agent.Packages.updatePackage(
                      artistDetails.memberId!,
                      artistPackage.artistPackageId!,
                      formData
                    );

                    var packages = await agent.Packages.getPackages(
                      artistDetails.memberId!
                    );

                    setPackages(packages);
                    dispatch(handleResetModal());
                  } catch (err: any) {
                    setSubmittingError(err);
                  }
                  setSubmitting(false);
                }}
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
                                <TextInput
                                  name="packageName"
                                  placeholder=""
                                  disabled={disabled}
                                />
                              </div>
                              <div className="mb-3">
                                <div className="d-flex">
                                  <p className="form-label">
                                    PACKAGE EQUIPMENT
                                  </p>
                                </div>
                                <p className="text-justify">
                                  Add the list of equipment you will provide
                                  with this package. Type your equipment name
                                  and press enter to add it to the list.
                                </p>
                                <EquipmentEntry
                                  equipment={equipment}
                                  setEquipment={setEquipment}
                                />
                              </div>
                              <div className="mb-3">
                                <div className="d-flex">
                                  <p className="form-label">
                                    PACKAGE DESCRIPTION
                                  </p>
                                </div>
                                <TextArea
                                  name="packageDescription"
                                  placeholder=""
                                  rows={7}
                                />
                              </div>
                              <div className="mb-3">
                                <div className="d-flex">
                                  <p className="form-label">
                                    PACKAGE PRICE PER HR
                                  </p>
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
                                Provide up to 3 photos of your equipment all
                                setup to help sell your package.
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
                        {!isSubmitting ? (
                          <Button
                            className={`black-button w-100 align-self-center`}
                            disabled={
                              images.length === 0 ||
                              !values.packageName ||
                              !values.packageDescription ||
                              !values.packagePrice ||
                              equipment.length === 0
                            }
                            onClick={() => submitForm()}
                          >
                            <h4>Update package</h4>
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
          )}
        </div>
      </>
    </div>
  );
};

export default EditArtistPackage;
