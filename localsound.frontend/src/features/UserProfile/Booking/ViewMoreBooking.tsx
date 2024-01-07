import { useLayoutEffect, useRef, useState } from "react";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import { BookingModel } from "./../../../app/model/dto/booking.model";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import BookingItem from "./BookingItem";
import BookingSummary from "./BookingSummary";
import { cp } from "fs";
import agent from "../../../api/agent";
import { debounce } from "lodash";

interface Props {
  bookingType: BookingTypes;
  pendingBookings: BookingModel[];
  setPendingBookings: (bookings: BookingModel[]) => void;
  cancelledBookings: BookingModel[];
  setCancelledBookings: (bookings: BookingModel[]) => void;
  upcomingBookings: BookingModel[];
  setUpcomingBookings: (bookings: BookingModel[]) => void;
}

const ViewMoreBooking = ({
  bookingType,
  pendingBookings,
  setPendingBookings,
  upcomingBookings,
  setUpcomingBookings,
  cancelledBookings,
  setCancelledBookings,
}: Props) => {
  const [bookings, setBookings] = useState<BookingModel[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState();
  const [page, setPage] = useState(0);
  const [canLoadMore, setCanLoadMore] = useState(false);
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();
  const listRef = useRef<HTMLDivElement>(null);

  window.onscroll = debounce(() => {
    if (listRef?.current) {
      const offset =
        window.innerHeight +
        document.documentElement.scrollTop -
        listRef?.current?.offsetHeight;

      if (
        !error &&
        canLoadMore &&
        !loading &&
        offset > 0 &&
        (offset < 102 || offset > 2200)
      ) {
        setPage(page + 1);
      }
    }
  }, 100);

  useLayoutEffect(() => {
    (async () => {
      setLoading(true);
      switch (bookingType) {
        case BookingTypes.upcoming: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page,
            true
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.pending: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page,
            null
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.cancelled: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page,
            false
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.completed: {
          var bookingResult = await agent.Bookings.getCompletedBookings(
            userDetails?.memberId!,
            page
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
      }
      setLoading(false);
    })();
  }, [page]);

  const getTitle = () => {
    switch (bookingType) {
      case BookingTypes.upcoming:
        return (
          <h3>
            <span className="black-highlight">Upcoming</span>
          </h3>
        );
      case BookingTypes.pending:
        return (
          <h3>
            <span className="black-highlight">Pending</span>
          </h3>
        );
      case BookingTypes.cancelled:
        return (
          <h3>
            <span className="black-highlight">Cancelled</span>
          </h3>
        );
      case BookingTypes.completed:
        return (
          <h3>
            <span className="black-highlight">Completed</span>
          </h3>
        );
    }
  };

  const OpenBookingInfo = (booking: BookingModel) => {
    dispatch(
      handleToggleModal({
        size: "small",
        body: <BookingItem booking={booking} bookingType={bookingType} />,
        open: true,
      })
    );
  };

  return (
    <div ref={listRef} className="component-container">
      {getTitle()}

      <>
        {error ? (
          <ErrorBanner className="mx-3" children={error} />
        ) : bookings.length > 0 ? (
          <>
            <div className="d-flex flex-row flex-wrap">
              {bookings.map((booking, index) => (
                <div
                  key={index}
                  className="px-3 col-12 mb-2"
                  onClick={() => OpenBookingInfo(booking)}
                >
                  <BookingSummary
                    booking={booking}
                    type={BookingTypes.upcoming}
                    user={userDetails!}
                    pendingBookings={pendingBookings}
                    setPendingBookings={setPendingBookings}
                    upcomingBookings={upcomingBookings}
                    setUpcomingBookings={setUpcomingBookings}
                    cancelledBookings={cancelledBookings}
                    setCancelledBookings={setCancelledBookings}
                  />
                </div>
              ))}
            </div>
          </>
        ) : null}
      </>
      {loading ? <InPageLoadingComponent /> : null}
    </div>
  );
};

export default ViewMoreBooking;
