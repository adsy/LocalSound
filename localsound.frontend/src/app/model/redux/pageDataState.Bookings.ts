import { BookingModel } from "../dto/booking.model";

export interface BookingOverviewState {
  upcoming: BookingModel[];
  pending: BookingModel[];
  completed: BookingModel[];
  cancelled: BookingModel[];
}
