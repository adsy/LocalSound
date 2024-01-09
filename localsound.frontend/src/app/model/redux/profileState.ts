import { UserModel } from "../dto/user.model";

export interface ProfileState {
  profileData: UserModel | null;
}
