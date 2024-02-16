import { useDispatch, useSelector } from "react-redux";
import {
  ArtistPackageModel,
  ArtistPackagePhotoModel,
} from "../../../../app/model/dto/artist-package.model";
import Label from "../../../../common/components/Label/Label";
import { UserModel } from "../../../../app/model/dto/user.model";
import { handleToggleModal } from "../../../../app/redux/actions/modalSlice";
import DeletePackageConfirmation from "./DeletePackageConfirmation";
import { Button } from "react-bootstrap";
import EditArtistPackage from "./EditArtistPackage";
import { Icon, Image as ImageComponent } from "semantic-ui-react";
import { State } from "../../../../app/model/redux/state";
import ImageDisplay from "../../../../common/imageDisplay/ImageDisplay";
import CreateBooking from "../../Booking/CreateBooking";
import { useLayoutEffect, useState } from "react";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";

interface Props {
  artistPackage: ArtistPackageModel;
  packages: ArtistPackageModel[];
  setPackages: (packages: ArtistPackageModel[]) => void;
  profileDetails: UserModel;
  selectedPackageId: string | null;
  setSelectedPackageId: (selectedPackageId: string | null) => void;
}

const ArtistPackageDisplay = ({
  artistPackage,
  packages,
  setPackages,
  profileDetails,
  selectedPackageId,
  setSelectedPackageId,
}: Props) => {
  const [imgsLoaded, setImgsLoaded] = useState(false);
  const [photos, setPhotos] = useState<string[]>([]);
  const loggedInUser = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const loadImage = (image: ArtistPackagePhotoModel) => {
    if (imgsLoaded) setImgsLoaded(false);
    return new Promise<string>((resolve, reject) => {
      const loadImg = new Image();
      loadImg.src = image.artistPackagePhotoUrl;
      loadImg.onload = () => resolve(image.artistPackagePhotoUrl);
      loadImg.onerror = (err) => reject(err);
      return loadImg;
    });
  };

  useLayoutEffect(() => {
    const IMAGES = [...artistPackage.photos];

    Promise.all(IMAGES.map((image) => loadImage(image)))
      .then((imgs: string[]) => {
        setPhotos(imgs);
      })
      .catch((err) => console.log("Failed to load images", err))
      .finally(() => {
        setImgsLoaded(true);
      });
  }, [artistPackage.photos]);

  const openDeleteModal = () => {
    dispatch(
      handleToggleModal({
        open: true,
        body: (
          <DeletePackageConfirmation
            profileDetails={profileDetails}
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
            profileDetails={profileDetails}
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
        body: (
          <CreateBooking
            artistPackage={selectedPackage}
            profileDetails={profileDetails}
          />
        ),
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
                {!imgsLoaded ? (
                  <InPageLoadingComponent />
                ) : artistPackage.photos ? (
                  artistPackage.photos.map((photo, index) => (
                    <ImageComponent
                      key={index}
                      size="small"
                      src={photo.artistPackagePhotoUrl}
                      className="package-image"
                    />
                  ))
                ) : null}
              </div>
            ) : null}
            <div className="d-flex flex-row flex-wrap justify-content-center">
              {artistPackage.equipment.map((equipment, index) => (
                <span className="badge-container" key={index}>
                  <Label
                    label={equipment.equipmentName}
                    id={equipment.equipmentId}
                  />
                </span>
              ))}
            </div>
            {profileDetails.memberId === loggedInUser?.memberId ? (
              <>
                <Button
                  className="white-button bin-icon"
                  onClick={() => openDeleteModal()}
                >
                  <Icon
                    name="trash"
                    size="small"
                    className="m-0 d-flex flex-row justify-content-center"
                  />
                </Button>

                <Button
                  className="white-button pencil-icon"
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
                  className="white-button close-icon fade-in"
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
                    <ImageComponent
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
            loggedInUser.memberId !== profileDetails.memberId ? (
              <Button
                className="white-button mt-2"
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
