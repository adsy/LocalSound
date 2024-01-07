import { useState } from "react";
import { BookingModel } from "../../../app/model/dto/booking.model";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { useDispatch, useSelector } from "react-redux";
import { handleResetModal } from "../../../app/redux/actions/modalSlice";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { State } from "../../../app/model/redux/state";
import agent from "../../../api/agent";

interface Props {
  isAccepting: boolean;
  booking: BookingModel;
  pendingBookings?: BookingModel[];
  setPendingBookings?: (bookings: BookingModel[]) => void;
  cancelledBookings?: BookingModel[];
  setCancelledBookings?: (bookings: BookingModel[]) => void;
  upcomingBookings?: BookingModel[];
  setUpcomingBookings?: (bookings: BookingModel[]) => void;
}

const AcceptCancelBookingModal = ({
  isAccepting,
  booking,
  pendingBookings,
  setPendingBookings,
  upcomingBookings,
  setUpcomingBookings,
  cancelledBookings,
  setCancelledBookings,
}: Props) => {
  const user = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState();

  const closeModal = () => {
    dispatch(handleResetModal());
  };

  const updatePendingBookings = () => {
    if (pendingBookings && setPendingBookings) {
      var clone = pendingBookings.filter(
        (x) => x.bookingId !== booking.bookingId
      );
      setPendingBookings(clone);
    }
  };

  const updateUpcomingBookings = () => {
    if (upcomingBookings && setUpcomingBookings) {
      var clone = upcomingBookings.filter(
        (x) => x.bookingId !== booking.bookingId
      );
      setUpcomingBookings(clone);
    }
  };

  const accept = async () => {
    try {
      setLoading(true);
      if (isAccepting) {
        await agent.Bookings.acceptBooking(user?.memberId!, booking.bookingId);
        updatePendingBookings();
        if (upcomingBookings && setUpcomingBookings) {
          booking.bookingConfirmed = true;
          setUpcomingBookings([booking, ...upcomingBookings]);
        }
      } else {
        await agent.Bookings.cancelBooking(user?.memberId!, booking.bookingId);
        if (booking.bookingConfirmed) {
          updateUpcomingBookings();
        } else {
          updatePendingBookings();
        }
        booking.bookingConfirmed = false;
        if (cancelledBookings && setCancelledBookings) {
          setCancelledBookings([booking, ...cancelledBookings]);
        }
      }
      dispatch(handleResetModal());
    } catch (err: any) {
      setError(err);
    }
    setLoading(false);
  };

  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">
          {isAccepting
            ? "Are you sure you want to accept this booking?"
            : "Are you sure you want to cancel this booking?"}
        </h2>
      </div>
      <div className="d-flex flex-column justify-content-center">
        {loading ? (
          <InPageLoadingComponent />
        ) : (
          <>
            <div className="d-flex flex-row">
              <a
                onClick={async () => await accept()}
                target="_blank"
                className="btn black-button save-crop-btn w-50"
              >
                <h4>Yes</h4>
              </a>
              <a
                onClick={() => closeModal()}
                target="_blank"
                className="ml-1 btn white-button save-crop-btn w-50"
              >
                <h4>No</h4>
              </a>
            </div>
          </>
        )}
      </div>
      {error ? <ErrorBanner children={error} className="mt-3 mb-0" /> : null}
    </div>
  );
};

export default AcceptCancelBookingModal;
