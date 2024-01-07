import CancelledBookings from "./CancelledBookings";
import CompletedBookings from "./CompletedBookings";
import PendingBookings from "./PendingBookings";
import UpcomingBookings from "./UpcomingBookings";
import { BookingModel } from "./../../../app/model/dto/booking.model";
import { useState } from "react";

const BookingsOverview = () => {
  const [upcomingBookings, setUpcomingBookings] = useState<BookingModel[]>([]);
  const [pendingBookings, setPendingBookings] = useState<BookingModel[]>([]);
  const [completedBookings, setCompletedBookings] = useState<BookingModel[]>(
    []
  );
  const [cancelledBookings, setCancelledBookings] = useState<BookingModel[]>(
    []
  );

  return (
    <div id="bookings" className="d-flex flex-row flex-wrap">
      <div className="col-12 col-lg-12 booking-container">
        <UpcomingBookings
          upcomingBookings={upcomingBookings}
          setUpcomingBookings={setUpcomingBookings}
          cancelledBookings={cancelledBookings}
          setCancelledBookings={setCancelledBookings}
        />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <PendingBookings
          pendingBookings={pendingBookings}
          setPendingBookings={setPendingBookings}
          upcomingBookings={upcomingBookings}
          setUpcomingBookings={setUpcomingBookings}
          cancelledBookings={cancelledBookings}
          setCancelledBookings={setCancelledBookings}
        />
      </div>
      <div className="col-12 col-lg-12 booking-container">
        <CompletedBookings
          completedBookings={completedBookings}
          setCompletedBookings={setCompletedBookings}
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
