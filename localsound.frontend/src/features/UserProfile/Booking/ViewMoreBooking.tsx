import { useEffect, useLayoutEffect, useState } from "react";
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
import {
  handleResetBookingOverviewData,
  handleSetCancelledBookings,
  handleSetCompletedBookings,
  handleSetPendingBookings,
  handleSetUpcomingBookings,
} from "../../../app/redux/actions/pageDataSlice";
import InfoBanner from "../../../common/banner/InfoBanner";

interface Props {
  bookingType: BookingTypes;
  setViewMore: (viewMore: BookingTypes | null) => void;
}

const ViewMoreBooking = ({ bookingType, setViewMore }: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const bookingData = useSelector((state: State) => state.pageData.bookingData);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState();
  const [canLoadMore, setCanLoadMore] = useState(false);
  const dispatch = useDispatch();

  useEffect(() => {
    return () => {
      dispatch(handleResetBookingOverviewData());
    };
  }, []);

  const getMoreBookings = async () => {
    setLoading(true);
    try {
      switch (bookingType) {
        case BookingTypes.upcoming: {
          var lastBookingId =
            bookingData.upcoming?.length > 0
              ? bookingData.upcoming[bookingData.upcoming.length - 1]?.bookingId
              : 0;

          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            lastBookingId,
            true
          );

          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(
            handleSetUpcomingBookings([
              ...bookingData.upcoming,
              ...bookingResult.bookings,
            ])
          );
          break;
        }
        case BookingTypes.pending: {
          var lastBookingId =
            bookingData.pending?.length > 0
              ? bookingData.pending[bookingData.pending.length - 1]?.bookingId
              : 0;

          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            lastBookingId,
            null
          );

          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(
            handleSetPendingBookings([
              ...bookingData.pending,
              ...bookingResult.bookings,
            ])
          );
          break;
        }
        case BookingTypes.cancelled: {
          var lastBookingId =
            bookingData.cancelled?.length > 0
              ? bookingData.cancelled[bookingData.cancelled.length - 1]
                  ?.bookingId
              : 0;

          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            lastBookingId,
            false
          );
          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(
            handleSetCancelledBookings([
              ...bookingData.cancelled,
              ...bookingResult.bookings,
            ])
          );
          break;
        }
        case BookingTypes.completed: {
          var lastBookingId =
            bookingData.completed?.length > 0
              ? bookingData.completed[bookingData.completed.length - 1]
                  ?.bookingId
              : 0;

          var bookingResult = await agent.Bookings.getCompletedBookings(
            userDetails?.memberId!,
            lastBookingId
          );
          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(
            handleSetCompletedBookings([
              ...bookingData.completed,
              ...bookingResult.bookings,
            ])
          );
          break;
        }
      }
    } catch (err: any) {
      setError(err);
    }
    setLoading(false);
  };

  useFixMissingScroll({
    hasMoreItems: canLoadMore,
    fetchMoreItems: getMoreBookings,
  });

  useLayoutEffect(() => {
    dispatch(handleResetBookingOverviewData());
    (async () => {
      setLoading(true);
      switch (bookingType) {
        case BookingTypes.upcoming: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            0,
            true
          );
          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(handleSetUpcomingBookings([...bookingResult.bookings]));
          break;
        }
        case BookingTypes.pending: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            0,
            null
          );
          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(handleSetPendingBookings([...bookingResult.bookings]));
          break;
        }
        case BookingTypes.cancelled: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            0,
            false
          );
          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(handleSetCancelledBookings([...bookingResult.bookings]));
          break;
        }
        case BookingTypes.completed: {
          var bookingResult = await agent.Bookings.getCompletedBookings(
            userDetails?.memberId!,
            0
          );
          setCanLoadMore(bookingResult.canLoadMore);
          dispatch(handleSetCompletedBookings([...bookingResult.bookings]));
          break;
        }
      }
      setLoading(false);
    })();
  }, []);

  const getBookings = () => {
    switch (bookingType) {
      case BookingTypes.upcoming: {
        return bookingData.upcoming;
      }
      case BookingTypes.pending: {
        return bookingData.pending;
      }
      case BookingTypes.cancelled: {
        return bookingData.cancelled;
      }
      case BookingTypes.completed: {
        return bookingData.completed;
      }
    }
  };

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

  const getInfoBannerText = () => {
    switch (bookingType) {
      case BookingTypes.pending:
        return "You currently have no pending bookings.";
      case BookingTypes.completed:
        return "You currently have no completed bookings.";
      case BookingTypes.cancelled:
        return "You currently have no cancelled bookings.";
      case BookingTypes.upcoming:
        return "You currently have no upcoming bookings.";
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
        ) : getBookings().length > 0 ? (
          <InfiniteScroll
            dataLength={getBookings().length} //This is important field to render the next data
            next={() => getMoreBookings()}
            hasMore={canLoadMore}
            loader={<></>}
          >
            {getBookings().map((booking, index) => {
              return (
                <div
                  key={index}
                  className="px-3 col-12 mb-2"
                  onClick={() => OpenBookingInfo(booking)}
                >
                  <BookingSummary
                    booking={booking}
                    type={bookingType}
                    user={userDetails!}
                  />
                </div>
              );
            })}
          </InfiniteScroll>
        ) : !loading ? (
          <InfoBanner className="fade-in mb-2 mx-3">
            <div className="d-flex flex-row justify-content-center align-items-center">
              <Icon
                name="calendar"
                size="small"
                className="follower-icon d-flex align-items-center justify-content-center"
              />
              <div className="ml-2">{getInfoBannerText()}</div>
            </div>
          </InfoBanner>
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
