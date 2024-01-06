import { userInfo } from "os";
import { BookingModel } from "../../../app/model/dto/booking.model";
import { UserModel } from "../../../app/model/dto/user.model";
import { BookingsTypes } from "../../../app/model/enums/BookingTypes";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import Label from "../../../common/components/Label/Label";
import { Icon } from "semantic-ui-react";

interface Props {
  booking: BookingModel;
  type: BookingsTypes;
  user: UserModel;
}

const BookingItem = ({ booking, type, user }: Props) => {
  const formatDate = (booking: Date) => {
    return `${booking.getDay()}/${
      booking.getMonth() + 1
    }/${booking.getFullYear()}`;
  };

  const calculatePrice = (price: string, length: number) => {
    return Number(price) * length;
  };

  const returnDateLabel = () => {
    return (
      <div className="d-flex flex-row align-items-center">
        <Icon name="calendar" size="small" />
        <span>{formatDate(new Date(booking.bookingDate))}</span>
      </div>
    );
  };

  return (
    <div className="booking-item col-12 col-lg-6">
      <div className="mb-3 d-flex flex-row justify-content-between align-items-center">
        <div className="name mr-2">
          {user.customerType === CustomerTypes.Artist
            ? booking.bookerName
            : booking.artistName}
        </div>
        <div className="name mr-2">
          <Label label={returnDateLabel()} id="0" color="black-badge" />
        </div>
      </div>
      <div className="date">
        {booking.packageName} - {booking.bookingLength} hours @ $
        {booking.packagePrice} per hr.
      </div>
      <div className="date">{booking.bookingAddress}</div>
    </div>
  );
};

export default BookingItem;
