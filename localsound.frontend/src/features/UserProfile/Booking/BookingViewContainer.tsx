import { useLayoutEffect, useState } from "react";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import { BookingModel } from "../../../app/model/dto/booking.model";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import BookingItemModal from "./BookingItemModal";
import BookingSummary from "./BookingSummary";
import agent from "../../../api/agent";
import { Button } from "react-bootstrap";
import { Icon } from "semantic-ui-react";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import InfoBanner from "../../../common/banner/InfoBanner";

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

const BookingViewContainer = ({
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
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

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
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.pending: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page,
            null
          );
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.cancelled: {
          var bookingResult = await agent.Bookings.getNonCompletedBookings(
            userDetails?.memberId!,
            page,
            false
          );
          setBookings([...bookings, ...bookingResult.bookings]);
          break;
        }
        case BookingTypes.completed: {
          var bookingResult = await agent.Bookings.getCompletedBookings(
            userDetails?.memberId!,
            page
          );
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

  const getText = () => {
    switch (bookingType) {
      case BookingTypes.pending:
        return `Click on a pending booking to view more information.
            ${
              userDetails?.customerType === CustomerTypes.Artist
                ? "You are able to accept or cancel whichever bookings you want directly from this page."
                : "If you do not need this booking anymore, you are able to cancel the booking directly from this page."
            }`;
      case BookingTypes.completed:
        return `Click on a completed booking to view more information.`;
      case BookingTypes.cancelled:
        return `Click on a cancelled booking to view more information.`;
      case BookingTypes.upcoming:
        return `Click on a completed booking to view more information. If you do not need this booking anymore, you are able to cancel the booking directly from this page.`;
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
      {getTitle()}
      {loading ? (
        <InPageLoadingComponent height={80} width={80} />
      ) : (
        <>
          <p className="px-3">{getText()}</p>
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
              </div>
              <div className="d-flex flex-row justify-content-center">
                <Button
                  className="mt-2 mx-3 black-button px-5"
                  onClick={() => setViewMore(bookingType)}
                  title="View more"
                >
                  <h4>View more</h4>
                </Button>
              </div>
            </>
          ) : (
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
          )}
        </>
      )}
    </div>
  );
};

export default BookingViewContainer;
