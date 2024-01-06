import { EquipmentModel } from "./equipment.model";

export interface BookingModel {
  bookingId: string;
  bookerId: string;
  bookerName: string;
  artistId: string;
  artistName: string;
  packageName: string;
  packagePrice: string;
  bookingDate: Date;
  bookingLength: number;
  bookingAddress: string;
  eventType: string;
  bookingDescription: string;
  packageEquipment: EquipmentModel[];
}
