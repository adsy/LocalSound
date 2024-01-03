import PendingBookings from "./PendingBookings";
import UpcomingBookings from "./UpcomingBookings";

const BookingsOverview = () => {
  return (
    <>
      <UpcomingBookings />
      <PendingBookings />
    </>
  );
};

export default BookingsOverview;
