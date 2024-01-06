import CancelledBookings from "./CancelledBookings";
import PastBookings from "./PastBookings";
import PendingBookings from "./PendingBookings";
import UpcomingBookings from "./UpcomingBookings";

const BookingsOverview = () => {
  return (
    <div id="bookings" className="d-flex flex-row flex-wrap">
      <div className="col-12 col-lg-12 booking-container">
        <UpcomingBookings />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <PendingBookings />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <CancelledBookings />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <PastBookings />
      </div>
      <div>calendar?</div>
    </div>
  );
};

export default BookingsOverview;
