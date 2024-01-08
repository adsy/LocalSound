import { useLayoutEffect, useState } from "react";
import agent from "../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { BookingModel } from "../../../app/model/dto/booking.model";
import BookingSummary from "./BookingSummary";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import BookingItem from "./BookingItem";
import { Button } from "react-bootstrap";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";

interface Props {
  pendingBookings: BookingModel[];
  setPendingBookings: (bookings: BookingModel[]) => void;
  cancelledBookings: BookingModel[];
  setCancelledBookings: (bookings: BookingModel[]) => void;
  upcomingBookings: BookingModel[];
  setUpcomingBookings: (bookings: BookingModel[]) => void;
  setViewMore: (bookingType: BookingTypes | null) => void;
}

const PendingBookings = ({
  pendingBookings,
  setPendingBookings,
  upcomingBookings,
  setUpcomingBookings,
  cancelledBookings,
  setCancelledBookings,
  setViewMore,
}: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useLayoutEffect(() => {
    (async () => {
      try {
        var bookings = await agent.Bookings.getNonCompletedBookings(
          userDetails?.memberId!,
          0,
          null
        );

        if (bookings) {
          setPendingBookings(bookings.bookings);
        }
      } catch (err: any) {
        setError(err);
      }
      setLoading(false);
    })();
  }, []);

  const OpenBookingInfo = (booking: BookingModel) => {
    dispatch(
      handleToggleModal({
        size: "small",
        body: (
          <BookingItem booking={booking} bookingType={BookingTypes.pending} />
        ),
        open: true,
      })
    );
  };

  return (
    <div className="component-container">
      <h3>
        <span className="black-highlight">Pending</span>
      </h3>
      {loading ? (
        <InPageLoadingComponent height={80} width={80} />
      ) : (
        <>
          <p className="px-3">
            Click on a pending booking to view more information.{" "}
            {userDetails?.customerType === CustomerTypes.Artist
              ? "You are able to accept or cancel whichever bookings you want directly from this page."
              : "If you do not need this booking anymore, you are able to cancel the booking directly from this page."}
          </p>
          {error ? (
            <ErrorBanner className="mx-3" children={error} />
          ) : pendingBookings.length > 0 ? (
            <>
              <div className="d-flex flex-row flex-wrap">
                {pendingBookings.map((booking, index) => (
                  <div
                    key={index}
                    className="px-3 col-12 mb-2"
                    onClick={() => OpenBookingInfo(booking)}
                  >
                    <BookingSummary
                      booking={booking}
                      type={BookingTypes.pending}
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
                  onClick={() => setViewMore(BookingTypes.pending)}
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
                <div className="ml-2">
                  You currently have no pending bookings.
                </div>
              </div>
            </InfoBanner>
          )}
        </>
      )}
    </div>
  );
};

export default PendingBookings;
