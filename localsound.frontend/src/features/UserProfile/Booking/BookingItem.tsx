import { BookingModel } from "../../../app/model/dto/booking.model";
import { BookingsTypes } from "../../../app/model/enums/BookingTypes";
import Label from "../../../common/components/Label/Label";
import { formatDate } from "../../../common/helper";

interface Props {
  booking: BookingModel;
  bookingType: BookingsTypes;
}

const BookingItem = ({ booking, bookingType }: Props) => {
  return (
    <div id="modal-popup">
      <div className="d-flex flex-row mb-4">
        <h2 className="header-title">{booking.packageName}</h2>
      </div>

      <div className="d-flex flex-row flex-wrap">
        <div className="col-6 col-xs-6">
          <div className="mb-3">
            <h4>Artist</h4>
            {booking.artistName}
          </div>

          <div className="mb-3">
            <h4>Package</h4>
            {booking.packageName}
          </div>

          <div className="mb-3">
            <h4>Event Type</h4>
            {booking.eventType}
          </div>

          <div className="mb-3">
            <h4>Booking length</h4>
            {booking.bookingLength} hours
          </div>
        </div>
        <div className="col-6 col-xs-6">
          <div className="mb-3">
            <h4>Booker</h4>
            {booking.bookerName}
          </div>

          <div className="mb-3">
            <h4>Package price</h4>${booking.packagePrice}
          </div>

          <div className="mb-3">
            <h4>Booking date</h4>
            {formatDate(new Date(booking.bookingDate))}
          </div>

          <div className="mb-3">
            <h4>Location</h4>
            {booking.bookingAddress}
          </div>
        </div>
      </div>

      <div className="mb-3">
        <h4>Additional info</h4>
        {booking.bookingDescription}
      </div>

      <div className="">
        <h4 className="mb-2">Equipment</h4>
        {booking.packageEquipment.map((equipment, index) => (
          <span className="badge-container" key={index}>
            <Label label={equipment.equipmentName} id={equipment.equipmentId} />
          </span>
        ))}
      </div>
    </div>
  );
};

export default BookingItem;
