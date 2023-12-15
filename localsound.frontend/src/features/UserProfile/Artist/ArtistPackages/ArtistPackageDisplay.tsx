import { useDispatch } from "react-redux";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import Label from "../../../../common/components/Label/Label";
import { UserModel } from "../../../../app/model/dto/user.model";
import { handleToggleModal } from "../../../../app/redux/actions/modalSlice";
import DeletePackageConfirmation from "./DeletePackageConfirmation";
import { Button } from "react-bootstrap";

interface Props {
  artistPackage: ArtistPackageModel;
  packages: ArtistPackageModel[];
  setPackages: (packages: ArtistPackageModel[]) => void;
  artistDetails: UserModel;
}

const ArtistPackageDisplay = ({
  artistPackage,
  packages,
  setPackages,
  artistDetails,
}: Props) => {
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

  return (
    <div id="artist-package-display" className="col-12 col-lg-4 px-2">
      <div className="component-container mt-0 ">
        <h2 className="text-center">
          <span className="purple-highlight">
            {artistPackage.artistPackageName}
          </span>
        </h2>
        <h3 className="text-center">
          ${artistPackage.artistPackagePrice} per hour
        </h3>
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
      </div>
      <Button className="white-button" onClick={() => openDeleteModal()}>
        Delete
      </Button>
    </div>
  );
};

export default ArtistPackageDisplay;
