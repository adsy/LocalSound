import { UserModel } from "../dto/user.model";
import { BookingOverviewState } from "./pageDataState.Bookings";

export interface PageDataState {
  profileData: UserModel | null;
  bookingData: BookingOverviewState;
}
