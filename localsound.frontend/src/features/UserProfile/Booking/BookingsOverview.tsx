import CancelledBookings from "./CancelledBookings";
import PastBookings from "./PastBookings";
import PendingBookings from "./PendingBookings";
import UpcomingBookings from "./UpcomingBookings";
import { BookingModel } from "./../../../app/model/dto/booking.model";
import { useState } from "react";

const BookingsOverview = () => {
  const [upcomingBookings, setUpcomingBookings] = useState<BookingModel[]>([]);
  const [pendingBookings, setPendingBookings] = useState<BookingModel[]>([]);
  const [pastBookings, setPastBookings] = useState<BookingModel[]>([]);
  const [cancelledBookings, setCancelledBookings] = useState<BookingModel[]>(
    []
  );

  return (
    <div id="bookings" className="d-flex flex-row flex-wrap">
      <div className="col-12 col-lg-12 booking-container">
        <UpcomingBookings
          upcomingBookings={upcomingBookings}
          setUpcomingBookings={setUpcomingBookings}
        />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <PendingBookings
          pendingBookings={pendingBookings}
          setPendingBookings={setPendingBookings}
        />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <PastBookings
          pastBookings={pastBookings}
          setPastBookings={setPastBookings}
        />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <CancelledBookings
          cancelledBookings={cancelledBookings}
          setCancelledBookings={setCancelledBookings}
        />
      </div>
      <div>calendar?</div>
    </div>
  );
};

export default BookingsOverview;
