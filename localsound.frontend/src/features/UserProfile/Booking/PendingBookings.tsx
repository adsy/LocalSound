import { useLayoutEffect, useState } from "react";
import agent from "../../../api/agent";
import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { BookingModel } from "../../../app/model/dto/booking.model";
import BookingSummary from "./BookingSummary";
import { BookingsTypes } from "../../../app/model/enums/BookingTypes";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";
import ErrorBanner from "../../../common/banner/ErrorBanner";

interface Props {
  pendingBookings: BookingModel[];
  setPendingBookings: (bookings: BookingModel[]) => void;
}

const PendingBookings = ({ pendingBookings, setPendingBookings }: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
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

  return (
    <div className="component-container">
      {loading ? (
        <InPageLoadingComponent />
      ) : (
        <>
          <h3>
            <span className="black-highlight">Pending</span>
          </h3>
          {pendingBookings.length > 0 ? (
            <div className="d-flex flex-column">
              {pendingBookings.map((booking, index) => (
                <div key={index} className="px-3">
                  <BookingSummary
                    booking={booking}
                    type={BookingsTypes.pending}
                    user={userDetails!}
                  />
                </div>
              ))}
            </div>
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
