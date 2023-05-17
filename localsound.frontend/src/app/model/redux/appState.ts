import { CustomerTypes } from "../enums/customerTypes";

interface AppState {
  appLoaded: boolean;
  userType: CustomerTypes | null;
}

export type { AppState };
