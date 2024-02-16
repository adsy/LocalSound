import { userInfo } from "os";
import { BookingModel } from "../../../app/model/dto/booking.model";
import { UserModel } from "../../../app/model/dto/user.model";
import { BookingTypes } from "../../../app/model/enums/BookingTypes";
import { CustomerTypes } from "../../../app/model/enums/customerTypes";
import Label from "../../../common/components/Label/Label";
import { Icon } from "semantic-ui-react";
import { formatDate } from "../../../common/helper";
import { Button } from "react-bootstrap";
import { useDispatch } from "react-redux";
import { handleToggleModal } from "../../../app/redux/actions/modalSlice";
import AcceptCancelBookingModal from "./AcceptCancelBookingModal";

interface Props {
  booking: BookingModel;
  type: BookingTypes;
  user: UserModel;
}

const BookingSummary = ({ booking, type, user }: Props) => {
  const dispatch = useDispatch();

  const returnDateLabel = () => {
    return (
      <div className="d-flex flex-row align-items-center">
        <Icon name="calendar" size="small" />
        <span>{formatDate(new Date(booking.bookingDate))}</span>
      </div>
    );
  };

  const openModal = (e: React.MouseEvent, isAccepting: boolean) => {
    e.stopPropagation();
    dispatch(
      handleToggleModal({
        size: "tiny",
        body: (
          <AcceptCancelBookingModal
            isAccepting={isAccepting}
            booking={booking}
          />
        ),
        open: true,
      })
    );
  };

  return (
    <div className="booking-item fade-in">
      <div className="mb-3 d-flex flex-row justify-content-between align-items-center gray-line pb-2 mb-2">
        <div className="name mr-2">
          {user.customerType === CustomerTypes.Artist
            ? booking.bookerName
            : booking.artistName}
        </div>
        <div className="name">
          <Label label={returnDateLabel()} id="0" color="black-badge" />
        </div>
      </div>
      <div className="date ">{booking.bookingAddress}</div>
      <div className="date">
        {booking.packageName} - {booking.bookingLength} hours @ $
        {booking.packagePrice} per hr.
      </div>
      <div className="d-flex flex-row justify-content-end">
        {user.customerType === CustomerTypes.Artist &&
        type === BookingTypes.pending ? (
          <Button
            className="white-button"
            onClick={(e) => openModal(e, true)}
            title="Accept booking"
          >
            <h4>
              <Icon
                name="check"
                size="small"
                className="m-0 d-flex flex-row justify-content-center"
              />
            </h4>
          </Button>
        ) : null}
        {type === BookingTypes.upcoming || type === BookingTypes.pending ? (
          <Button
            className="white-button ml-1"
            onClick={(e) => openModal(e, false)}
            title="Cancel booking"
          >
            <h4>
              <Icon
                name="cancel"
                size="small"
                className="m-0 d-flex flex-row justify-content-center"
              />
            </h4>
          </Button>
        ) : null}
      </div>
    </div>
  );
};

export default BookingSummary;
