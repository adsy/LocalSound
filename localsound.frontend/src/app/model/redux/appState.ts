import { CustomerTypes } from "../enums/customerTypes";

interface AppState {
  appLoading: boolean;
  userType: CustomerTypes | null;
}

export type { AppState };
