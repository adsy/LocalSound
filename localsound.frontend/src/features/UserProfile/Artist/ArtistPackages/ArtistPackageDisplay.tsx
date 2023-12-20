import { useDispatch, useSelector } from "react-redux";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import Label from "../../../../common/components/Label/Label";
import { UserModel } from "../../../../app/model/dto/user.model";
import { handleToggleModal } from "../../../../app/redux/actions/modalSlice";
import DeletePackageConfirmation from "./DeletePackageConfirmation";
import { Button } from "react-bootstrap";
import EditArtistPackage from "./EditArtistPackage";
import { Icon, Image } from "semantic-ui-react";
import { State } from "../../../../app/model/redux/state";
import ImageDisplay from "../../../../common/imageDisplay/ImageDisplay";
import CreateBooking from "../../Booking/CreateBooking";

interface Props {
  artistPackage: ArtistPackageModel;
  packages: ArtistPackageModel[];
  setPackages: (packages: ArtistPackageModel[]) => void;
  artistDetails: UserModel;
  selectedPackageId: string | null;
  setSelectedPackageId: (selectedPackageId: string | null) => void;
}

const ArtistPackageDisplay = ({
  artistPackage,
  packages,
  setPackages,
  artistDetails,
  selectedPackageId,
  setSelectedPackageId,
}: Props) => {
  const loggedInUser = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const openDeleteModal = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <DeletePackageConfirmation
            artistDetails={artistDetails}
            artistPackage={artistPackage}
            packages={packages}
            setPackages={setPackages}
          />
        ),
        size: "tiny",
      })
    );
  };

  const openEditModal = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <EditArtistPackage
            artistDetails={artistDetails}
            artistPackage={artistPackage}
            setPackages={setPackages}
          />
        ),
        size: "large",
      })
    );
  };

  const openImageModal = (url: string) => {
    dispatch(
      handleToggleModal({
        open: true,
        body: <ImageDisplay imageUrl={url} />,
        size: "small",
      })
    );
  };

  const openBookingModal = (selectedPackage: ArtistPackageModel) => {
    dispatch(
      handleToggleModal({
        open: true,
        body: <CreateBooking artistPackage={selectedPackage} />,
        size: "tiny",
      })
    );
  };

  const closePackageDetail = () => {
    setSelectedPackageId(null);
  };

  const viewMoreInfo = (artistPackageId: string) => {
    setSelectedPackageId(artistPackageId);
  };

  return (
    <>
      <div className="component-container mt-0 position-relative h-100 d-flex flex-row flex-wrap justify-content-between">
        <div
          className={`${
            selectedPackageId !== artistPackage.artistPackageId
              ? "col-12"
              : "col-12 col-lg-5"
          } d-flex flex-column justify-content-between`}
        >
          <div className="d-flex flex-column">
            <h2 className="text-center">
              <span className="black-highlight">
                {artistPackage.artistPackageName}
              </span>
            </h2>
            <h4 className="text-center mb-4">
              <span className="purple-highlight">
                ${artistPackage.artistPackagePrice} per hour
              </span>
            </h4>
            {selectedPackageId !== artistPackage.artistPackageId ? (
              <div className="images-container d-flex flex-row justify-content-center fade-in">
                {artistPackage.photos
                  ? artistPackage.photos.map((photo, index) => (
                      <Image
                        key={index}
                        size="small"
                        src={photo.artistPackagePhotoUrl}
                        className="package-image"
                      />
                    ))
                  : null}
              </div>
            ) : null}
            <div className="d-flex flex-row flex-wrap justify-content-center">
              {artistPackage.equipment.map((equipment, index) => (
                <span key={index}>
                  <Label
                    label={equipment.equipmentName}
                    id={equipment.equipmentId}
                  />
                </span>
              ))}
            </div>
            {artistDetails.memberId === loggedInUser?.memberId ? (
              <>
                <Button
                  className="black-button bin-icon"
                  onClick={() => openDeleteModal()}
                >
                  <Icon
                    name="trash"
                    size="small"
                    className="m-0 d-flex flex-row justify-content-center"
                  />
                </Button>

                <Button
                  className="black-button pencil-icon"
                  onClick={() => openEditModal()}
                >
                  <Icon
                    name="pencil"
                    size="small"
                    className="m-0 d-flex flex-row justify-content-center"
                  />
                </Button>
              </>
            ) : null}
            <>
              {selectedPackageId === artistPackage.artistPackageId ? (
                <Button
                  className="black-button close-icon fade-in"
                  onClick={() => closePackageDetail()}
                >
                  <Icon
                    name="close"
                    size="small"
                    className="m-0 d-flex flex-row justify-content-center"
                  />
                </Button>
              ) : null}
            </>
          </div>
          {selectedPackageId === artistPackage.artistPackageId ? (
            <div className="images-container d-flex flex-row justify-content-center fade-in px-5 mb-3">
              {artistPackage.photos
                ? artistPackage.photos.map((photo, index) => (
                    <Image
                      key={index}
                      size="small"
                      src={photo.artistPackagePhotoUrl}
                      className="expanded-image fade-in"
                      onClick={() =>
                        openImageModal(photo.artistPackagePhotoUrl)
                      }
                    />
                  ))
                : null}
            </div>
          ) : null}
          <div className="d-flex flex-column mt-2">
            {loggedInUser &&
            loggedInUser.memberId !== artistDetails.memberId ? (
              <Button
                className="black-button mt-2"
                onClick={() => openBookingModal(artistPackage)}
              >
                <h4>Make booking</h4>
              </Button>
            ) : null}
            {selectedPackageId !== artistPackage.artistPackageId ? (
              <Button
                className="white-button mt-2"
                onClick={() => viewMoreInfo(artistPackage.artistPackageId!)}
              >
                <h4>View more information</h4>
              </Button>
            ) : null}
          </div>
        </div>
        {selectedPackageId === artistPackage.artistPackageId ? (
          <div className="col-12 col-lg-6 description">
            {artistPackage.artistPackageDescription}
          </div>
        ) : null}
      </div>
    </>
  );
};

export default ArtistPackageDisplay;
