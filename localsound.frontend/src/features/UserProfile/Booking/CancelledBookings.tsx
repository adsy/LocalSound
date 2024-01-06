import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { useLayoutEffect, useState } from "react";
import agent from "../../../api/agent";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { BookingModel } from "../../../app/model/dto/booking.model";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";

interface Props {
  cancelledBookings: BookingModel[];
  setCancelledBookings: (bookings: BookingModel[]) => void;
}

const CancelledBookings = ({
  cancelledBookings,
  setCancelledBookings,
}: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useLayoutEffect(() => {
    (async () => {
      try {
        var bookings = await agent.Bookings.getFutureBookings(
          userDetails?.memberId!,
          0,
          false
        );

        setCancelledBookings(bookings);
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
            <span className="black-highlight">Cancelled</span>
          </h3>
          {cancelledBookings.length > 0 ? (
            <div></div>
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
