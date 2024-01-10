import { useLayoutEffect, useState } from "react";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import { BookingModel } from "./../../../app/model/dto/booking.model";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import BookingItemModal from "./BookingItemModal";
import BookingSummary from "./BookingSummary";
import agent from "../../../api/agent";
import InfiniteScroll from "react-infinite-scroll-component";
import { Button } from "react-bootstrap";
import { Icon } from "semantic-ui-react";
import useFixMissingScroll from "../../../common/hooks/UseLoadMoreWithoutScroll";

interface Props {
  bookingType: BookingTypes;
  pendingBookings: BookingModel[];
  setPendingBookings: (bookings: BookingModel[]) => void;
  cancelledBookings: BookingModel[];
  setCancelledBookings: (bookings: BookingModel[]) => void;
  upcomingBookings: BookingModel[];
  setUpcomingBookings: (bookings: BookingModel[]) => void;
  setViewMore: (viewMore: BookingTypes | null) => void;
}

const ViewMoreBooking = ({
  bookingType,
  pendingBookings,
  setPendingBookings,
  upcomingBookings,
  setUpcomingBookings,
  cancelledBookings,
  setCancelledBookings,
  setViewMore,
}: Props) => {
  const [bookings, setBookings] = useState<BookingModel[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState();
  const [page, setPage] = useState(0);
  const [canLoadMore, setCanLoadMore] = useState(false);
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const getMoreBookings = async () => {
    try {
      setLoading(true);
      switch (bookingType) {
        case BookingTypes.upcoming: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page + 1,
            true
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.pending: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page + 1,
            null
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.cancelled: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page + 1,
            false
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.completed: {
          var bookingResult = await agent.Bookings.getCompletedBookings(
            userDetails?.memberId!,
            page + 1
          );
          setCanLoadMore(bookingResult.canLoadMore);
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
      }
      setPage(page + 1);
      setLoading(false);
    } catch (err: any) {
      setError(err);
    }
  };

  useFixMissingScroll({
    hasMoreItems: canLoadMore,
    fetchMoreItems: getMoreBookings,
  });

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
  }, []);

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
        body: <BookingItemModal booking={booking} bookingType={bookingType} />,
        open: true,
      })
    );
  };

  return (
    <div className="component-container">
      <div className="d-flex flex-row justify-content-between mb-3 align-items-center">
        {getTitle()}
        <Button
          className="white-button"
          onClick={() => setViewMore(null)}
          title="Go back"
        >
          <h4 className="d-flex flex-row align-items-center">
            <Icon name="arrow left" size="small" className="m-0" />
            <span className="ml-1">Go back</span>
          </h4>
        </Button>
      </div>

      <>
        {error ? (
          <ErrorBanner className="mx-3" children={error} />
        ) : bookings.length > 0 ? (
          <InfiniteScroll
            dataLength={bookings.length} //This is important field to render the next data
            next={() => getMoreBookings()}
            hasMore={canLoadMore}
            loader={<></>}
          >
            {bookings.map((booking, index) => (
              <div
                key={index}
                className="px-3 col-12 mb-2"
                onClick={() => OpenBookingInfo(booking)}
              >
                <BookingSummary
                  booking={booking}
                  type={bookingType}
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
          </InfiniteScroll>
        ) : null}
      </>
      {loading ? (
        <div className="mt-2 mb-1">
          <InPageLoadingComponent height={80} width={80} />
        </div>
      ) : null}
    </div>
  );
};

export default ViewMoreBooking;
