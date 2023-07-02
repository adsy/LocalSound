import { CustomerTypes } from "../enums/customerTypes";

export interface UserRegistrationModel {
  customerType: CustomerTypes;
  registrationDto: RegistrationModel;
}

export interface RegistrationModel {
  profileUrl: string;
  email: string;
  password: string;
  checkPassword: string;
  firstName?: string;
  lastName?: string;
  address: string;
  phoneNumber: string;
  name?: string;
  soundcloudUrl?: string;
  spotifyUrl?: string;
  youtubeUrl?: string;
}
