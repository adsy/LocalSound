import { AccountImageModel } from "./account-image.model";

export interface UserSummaryModel {
  memberId: string;
  name: string;
  profileUrl: string;
  images: AccountImageModel[];
}
