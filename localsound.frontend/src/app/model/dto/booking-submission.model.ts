export interface BookingSubmissionModel {
  artistId: string;
  packageId: string;
  eventTypeId: string;
  bookingDescription: string;
  bookingAddress: string;
  bookingLength: number;
  bookingDate: Date;
}
