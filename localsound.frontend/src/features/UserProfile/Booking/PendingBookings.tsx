import { useLayoutEffect, useState } from "react";
import agent from "../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { BookingModel } from "../../../app/model/dto/booking.model";
import BookingSummary from "./BookingSummary";
import { BookingsTypes } from "../../../app/model/enums/BookingTypes";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import BookingItem from "./BookingItem";
import { Button } from "react-bootstrap";

interface Props {
  pendingBookings: BookingModel[];
  setPendingBookings: (bookings: BookingModel[]) => void;
}

const PendingBookings = ({ pendingBookings, setPendingBookings }: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useLayoutEffect(() => {
    (async () => {
      try {
        var bookings = await agent.Bookings.getFutureBookings(
          userDetails?.memberId!,
          0,
          null
        );

        if (bookings) {
          setPendingBookings(bookings);
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
          <BookingItem booking={booking} bookingType={BookingsTypes.pending} />
        ),
        open: true,
      })
    );
  };

  return (
    <div className="component-container">
      {loading ? (
        <InPageLoadingComponent />
      ) : (
        <div className="d-flex flex-column">
          <h3>
            <span className="black-highlight">Pending</span>
          </h3>
          <p className="px-3">
            Click on a pending booking to view more information.{" "}
          </p>
          {pendingBookings.length > 0 ? (
            <>
              <div className="d-flex flex-row flex-wrap">
                {pendingBookings.map((booking, index) => (
                  <div
                    key={index}
                    className="px-3 col-12 col-lg-6 mb-2"
                    onClick={() => OpenBookingInfo(booking)}
                  >
                    <BookingSummary
                      booking={booking}
                      type={BookingsTypes.pending}
                      user={userDetails!}
                    />
                  </div>
                ))}
              </div>
              <div className="d-flex flex-row justify-content-center">
                <Button className="mt-2 mx-3 black-button px-5">
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
        </div>
      )}
    </div>
  );
};

export default PendingBookings;
