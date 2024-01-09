import { BookingModel } from "./../../../app/model/dto/booking.model";
import { useEffect, useState } from "react";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import ViewMoreBooking from "./ViewMoreBooking";
import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import BookingViewContainer from "./BookingViewContainer";

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
  const appLoading = useSelector((state: State) => state.app.appLoading);

  useEffect(() => {
    if (viewMore !== null) {
      setShowMore(true);
    } else {
      setShowMore(false);
    }
  }, [viewMore]);

  return (
    <div id="bookings" className="d-flex flex-row flex-wrap">
      {appLoading ? null : (
        <>
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
                setViewMore={setViewMore}
              />
            </div>
          ) : (
            <>
              <div className="col-12 col-lg-12 booking-container">
                <BookingViewContainer
                  pendingBookings={pendingBookings}
                  setPendingBookings={setPendingBookings}
                  upcomingBookings={upcomingBookings}
                  setUpcomingBookings={setUpcomingBookings}
                  cancelledBookings={cancelledBookings}
                  setCancelledBookings={setCancelledBookings}
                  setViewMore={setViewMore}
                  bookingType={BookingTypes.upcoming}
                />
              </div>
              <div className="col-12 col-lg-12 booking-container">
                <BookingViewContainer
                  pendingBookings={pendingBookings}
                  setPendingBookings={setPendingBookings}
                  upcomingBookings={upcomingBookings}
                  setUpcomingBookings={setUpcomingBookings}
                  cancelledBookings={cancelledBookings}
                  setCancelledBookings={setCancelledBookings}
                  setViewMore={setViewMore}
                  bookingType={BookingTypes.pending}
                />
              </div>
              <div className="col-12 col-lg-12 booking-container">
                <BookingViewContainer
                  pendingBookings={pendingBookings}
                  setPendingBookings={setPendingBookings}
                  upcomingBookings={upcomingBookings}
                  setUpcomingBookings={setUpcomingBookings}
                  cancelledBookings={cancelledBookings}
                  setCancelledBookings={setCancelledBookings}
                  setViewMore={setViewMore}
                  bookingType={BookingTypes.completed}
                />
              </div>
              <div className="col-12 col-lg-12 booking-container">
                <BookingViewContainer
                  pendingBookings={pendingBookings}
                  setPendingBookings={setPendingBookings}
                  upcomingBookings={upcomingBookings}
                  setUpcomingBookings={setUpcomingBookings}
                  cancelledBookings={cancelledBookings}
                  setCancelledBookings={setCancelledBookings}
                  setViewMore={setViewMore}
                  bookingType={BookingTypes.cancelled}
                />
              </div>
            </>
          )}
        </>
      )}
      {/* <div>calendar?</div> */}
    </div>
  );
};

export default BookingsOverview;
