import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { useLayoutEffect, useState } from "react";
import agent from "../../../api/agent";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";
import { BookingModel } from "../../../app/model/dto/booking.model";
import InfoBanner from "../../../common/banner/InfoBanner";
import { Icon } from "semantic-ui-react";

interface Props {
  pastBookings: BookingModel[];
  setPastBookings: (bookings: BookingModel[]) => void;
}

const PastBookings = ({ pastBookings, setPastBookings }: Props) => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useLayoutEffect(() => {
    (async () => {
      try {
        // var bookings = await agent.Bookings.getFutureBookings(
        //   userDetails?.memberId!,
        //   0,
        //   true
        // );
        // setPastBookings(bookings);
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
            <span className="black-highlight">Past</span>
          </h3>
          {pastBookings.length > 0 ? (
            <div></div>
          ) : (
            <InfoBanner className="fade-in mb-2 mx-3">
              <div className="d-flex flex-row justify-content-center align-items-center">
                <Icon
                  name="calendar"
                  size="small"
                  className="follower-icon d-flex align-items-center justify-content-center"
                />
                <div className="ml-2">You currently have no past bookings.</div>
              </div>
            </InfoBanner>
          )}
        </>
      )}
    </div>
  );
};

export default PastBookings;
