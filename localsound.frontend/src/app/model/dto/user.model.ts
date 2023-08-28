import { CustomerTypes } from "../enums/customerTypes";

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
}
