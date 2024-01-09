import { CustomerTypes } from "../enums/customerTypes";
import { AccountImageModel } from "./account-image.model";
import { EquipmentModel } from "./equipment.model";
import { EventTypeModel } from "./eventType.model";
import { GenreModel } from "./genre.model";

export interface UserModel {
  customerType: CustomerTypes;
  memberId: string;
  email: string;
  profileUrl: string;
  address: string;
  phoneNumber: string;
  firstName?: string;
  lastName?: string;
  name?: string;
  soundcloudUrl?: string;
  spotifyUrl?: string;
  youtubeUrl?: string;
  aboutSection?: string;
  emailConfirmed: boolean;
  genres: GenreModel[];
  equipment: EquipmentModel[];
  eventTypes: EventTypeModel[];
  images: AccountImageModel[];
  followerCount: number;
  followingCount: number;
  isFollowing?: boolean;
  canAddPackage: boolean;
}
