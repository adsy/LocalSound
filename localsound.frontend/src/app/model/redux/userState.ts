import { UserModel } from "../dto/user.model";

export interface UserState {
  userDetails: UserModel | null;
}
