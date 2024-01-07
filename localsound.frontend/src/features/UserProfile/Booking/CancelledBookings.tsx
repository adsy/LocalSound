import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { useLayoutEffect, useState } from "react";
import agent from "../../../api/agent";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { BookingModel } from "../../../app/model/dto/booking.model";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import BookingItem from "./BookingItem";
import BookingSummary from "./BookingSummary";
import { Button } from "react-bootstrap";
import ErrorBanner from "../../../common/banner/ErrorBanner";

interface Props {
  cancelledBookings: BookingModel[];
  setCancelledBookings: (bookings: BookingModel[]) => void;
  setViewMore: (bookingType: BookingTypes | null) => void;
}

const CancelledBookings = ({
  cancelledBookings,
  setCancelledBookings,
  setViewMore,
}: Props) => {
  const dispatch = useDispatch();
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useLayoutEffect(() => {
    (async () => {
      try {
        var bookings = await agent.Bookings.getNonCompletedBookings(
          userDetails?.memberId!,
          0,
          false
        );

        setCancelledBookings(bookings.bookings);
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
        <span className="black-highlight">Cancelled</span>
      </h3>
      {loading ? (
        <InPageLoadingComponent />
      ) : (
        <>
          {error ? (
            <ErrorBanner children={error} />
          ) : cancelledBookings.length > 0 ? (
            <>
              <div className="d-flex flex-row flex-wrap">
                {cancelledBookings.map((booking, index) => (
                  <div
                    key={index}
                    className="px-3 col-12 mb-2"
                    onClick={() => OpenBookingInfo(booking)}
                  >
                    <BookingSummary
                      booking={booking}
                      type={BookingTypes.cancelled}
                      user={userDetails!}
                      cancelledBookings={cancelledBookings}
                      setCancelledBookings={setCancelledBookings}
                    />
                  </div>
                ))}
              </div>
              <div className="d-flex flex-row justify-content-center">
                <Button
                  className="mt-2 mx-3 black-button px-5"
                  onClick={() => setViewMore(BookingTypes.cancelled)}
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
                  You currently have no cancelled bookings.
                </div>
              </div>
            </InfoBanner>
          )}
        </>
      )}
    </div>
  );
};

export default CancelledBookings;
