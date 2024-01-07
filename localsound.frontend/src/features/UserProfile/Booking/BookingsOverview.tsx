import CancelledBookings from "./CancelledBookings";
import CompletedBookings from "./CompletedBookings";
import PendingBookings from "./PendingBookings";
import UpcomingBookings from "./UpcomingBookings";
import { BookingModel } from "./../../../app/model/dto/booking.model";
import { useEffect, useState } from "react";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import ViewMoreBooking from "./ViewMoreBooking";

const BookingsOverview = () => {
  const [upcomingBookings, setUpcomingBookings] = useState<BookingModel[]>([]);
  const [pendingBookings, setPendingBookings] = useState<BookingModel[]>([]);
  const [completedBookings, setCompletedBookings] = useState<BookingModel[]>(
    []
  );
  const [cancelledBookings, setCancelledBookings] = useState<BookingModel[]>(
    []
  );
  const [showViewMore, setShowMore] = useState(false);
  const [viewMore, setViewMore] = useState<BookingTypes | null>(null);

  useEffect(() => {
    if (viewMore !== null) {
      setShowMore(true);
    } else {
      setShowMore(false);
    }
  }, [viewMore]);

  return (
    <div id="bookings" className="d-flex flex-row flex-wrap">
      {showViewMore && viewMore !== null ? (
        <div className="col-12 col-lg-12 booking-container">
          <ViewMoreBooking
            bookingType={viewMore}
            pendingBookings={pendingBookings}
            setPendingBookings={setPendingBookings}
            upcomingBookings={upcomingBookings}
            setUpcomingBookings={setUpcomingBookings}
            cancelledBookings={cancelledBookings}
            setCancelledBookings={setCancelledBookings}
          />
        </div>
      ) : (
        <>
          <div className="col-12 col-lg-12 booking-container">
            <UpcomingBookings
              upcomingBookings={upcomingBookings}
              setUpcomingBookings={setUpcomingBookings}
              cancelledBookings={cancelledBookings}
              setCancelledBookings={setCancelledBookings}
              setViewMore={setViewMore}
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
              setViewMore={setViewMore}
            />
          </div>
          <div className="col-12 col-lg-12 booking-container">
            <CompletedBookings
              completedBookings={completedBookings}
              setCompletedBookings={setCompletedBookings}
              setViewMore={setViewMore}
            />
          </div>
          <div className="col-12 col-lg-12 booking-container">
            <CancelledBookings
              cancelledBookings={cancelledBookings}
              setCancelledBookings={setCancelledBookings}
              setViewMore={setViewMore}
            />
          </div>
        </>
      )}
      {/* <div>calendar?</div> */}
    </div>
  );
};

export default BookingsOverview;
