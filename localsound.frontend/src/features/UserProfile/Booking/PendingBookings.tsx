import { useLayoutEffect, useState } from "react";
import { BookingModel } from "../../../app/model/dto/booking.model";
import agent from "../../../api/agent";
import { useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";

const PendingBookings = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [pendingBookings, setPendingBookings] = useState<BookingModel[]>([]);

  useLayoutEffect(() => {
    (async () => {
      try {
        var bookings = await agent.Bookings.getFutureBookings(
          userDetails?.memberId!,
          0,
          null
        );

        setPendingBookings(bookings);
      } catch (err: any) {
        setError(err);
      }
      setLoading(false);
    })();
  }, []);

  return (
    <div className="component-container">
      {loading ? <InPageLoadingComponent /> : <div>pending</div>}
    </div>
  );
};

export default PendingBookings;
