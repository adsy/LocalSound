import { useState } from "react";
import { useDispatch } from "react-redux";
import { handleResetModal } from "../../../../app/redux/actions/modalSlice";
import { ArtistPackageModel } from "../../../../app/model/dto/artist-package.model";
import { UserModel } from "../../../../app/model/dto/user.model";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import agent from "../../../../api/agent";
import { handleSetUserDetails } from "../../../../app/redux/actions/userSlice";

interface Props {
  artistPackage: ArtistPackageModel;
  packages: ArtistPackageModel[];
  setPackages: (packages: ArtistPackageModel[]) => void;
  artistDetails: UserModel;
}

const DeletePackageConfirmation = ({
  artistPackage,
  packages,
  setPackages,
  artistDetails,
}: Props) => {
  const dispatch = useDispatch();
  const [deleteError, setDeleteError] = useState<string | null>();
  const [deletingPackage, setDeletingPackage] = useState(false);

  const cancelDelete = () => {
    dispatch(handleResetModal());
  };

  const deleteTrack = async () => {
    setDeleteError(null);
    try {
      if (artistDetails?.memberId && artistPackage.artistPackageId) {
        setDeletingPackage(true);
        await agent.Packages.deletePackage(
          artistDetails?.memberId,
          artistPackage.artistPackageId
        );

        var packagesFiltered = packages.filter(
          (x) => x.artistPackageId !== artistPackage.artistPackageId
        );
        setPackages(packagesFiltered);

        if (!artistDetails.canAddPackage) {
          var clone = { ...artistDetails };
          clone.canAddPackage = true;
          dispatch(handleSetUserDetails(clone));
        }

        dispatch(handleResetModal());
      }
    } catch (err: any) {
      setDeleteError(err);
      setDeletingPackage(false);
    }
  };

  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">
          Are you sure you want to delete this package?
        </h2>
      </div>
      <div className="d-flex flex-row justify-content-center">
        {deletingPackage ? (
          <InPageLoadingComponent />
        ) : (
          <>
            <a
              onClick={async () => await deleteTrack()}
              target="_blank"
              className="btn black-button save-crop-btn w-50"
            >
              <h4>Yes</h4>
            </a>
            <a
              onClick={() => cancelDelete()}
              target="_blank"
              className="ml-1 btn white-button save-crop-btn w-50"
            >
              <h4>No</h4>
            </a>
          </>
        )}
      </div>
      {deleteError ? (
        <ErrorBanner children={deleteError} className="mt-3 mb-0" />
      ) : null}
    </div>
  );
};

export default DeletePackageConfirmation;
