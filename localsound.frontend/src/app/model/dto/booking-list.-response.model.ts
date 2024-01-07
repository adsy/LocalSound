import { BookingModel } from "./booking.model";

export interface BookingListResponse {
  bookings: BookingModel[];
  canLoadMore: boolean;
}
