import { useDispatch } from "react-redux";
import { ArtistTrackModel } from "../../../../app/model/dto/artist-track-upload.model";
import { handleResetModal } from "../../../../app/redux/actions/modalSlice";
import { useState } from "react";
import agent from "../../../../api/agent";
import { UserModel } from "../../../../app/model/dto/user.model";
import ErrorBanner from "../../../../common/banner/ErrorBanner";
import InPageLoadingComponent from "../../../../app/layout/InPageLoadingComponent";
import { handleTrackDeleted } from "../../../../app/redux/actions/pageOperationSlice";

interface Props {
  track: ArtistTrackModel;
  tracks: ArtistTrackModel[];
  setTracks: (tracks: ArtistTrackModel[]) => void;
  loggedInUser: UserModel;
}

const DeleteTrackConfirmation = ({
  track,
  tracks,
  setTracks,
  loggedInUser,
}: Props) => {
  const dispatch = useDispatch();
  const [deleteError, setDeleteError] = useState<string | null>();
  const [deletingTrack, setDeletingTrack] = useState(false);

  const cancelDelete = () => {
    dispatch(handleResetModal());
  };

  const deleteTrack = async () => {
    dispatch(handleTrackDeleted(false));
    setDeleteError(null);
    try {
      if (loggedInUser?.memberId && track.artistTrackId) {
        setDeletingTrack(true);
        await agent.Tracks.deleteTrack(
          loggedInUser?.memberId,
          track.artistTrackId
        );

        var tracksFiltered = tracks.filter(
          (x) => x.artistTrackId !== track.artistTrackId
        );
        setTracks(tracksFiltered);
        dispatch(handleTrackDeleted(true));
        dispatch(handleResetModal());
      }
    } catch (err: any) {
      setDeleteError(err);
      setDeletingTrack(false);
    }
  };

  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">
          Are you sure you want to delete this track?
        </h2>
      </div>
      <div className="d-flex flex-row justify-content-center">
        {deletingTrack ? (
          <InPageLoadingComponent />
        ) : (
          <>
            <a
              onClick={async () => await deleteTrack()}
              target="_blank"
              className="btn white-button save-crop-btn w-50"
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

export default DeleteTrackConfirmation;
