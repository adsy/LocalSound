import { BookingModel } from "../../../app/model/dto/booking.model";

interface Props {
  booking: BookingModel;
}

const BookingItem = ({ booking }: Props) => {
  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">{booking.artistName}</h2>
      </div>
      {/* <div className="d-flex flex-row justify-content-center">
        {deletingTrack ? (
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
      ) : null} */}
    </div>
  );
};

export default BookingItem;
