import { UserSummaryModel } from "./user-summary.model";

export interface FollowerListResponse {
  followers: UserSummaryModel[];
  canLoadMore: boolean;
}
