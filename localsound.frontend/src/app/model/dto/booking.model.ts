import { EquipmentModel } from "./equipment.model";

export interface BookingModel {
  bookingId: number;
  bookerId: string;
  bookerName: string;
  artistId: string;
  artistName: string;
  packageName: string;
  packagePrice: string;
  bookingDate: Date;
  bookingLength: number;
  bookingAddress: string;
  bookingConfirmed?: boolean;
  eventType: string;
  bookingDescription: string;
  packageEquipment: EquipmentModel[];
}
