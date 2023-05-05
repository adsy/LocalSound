import { CustomerTypes } from "../enums/customerTypes";

interface AuthenticationPage {
  customerType: CustomerTypes | null;
  addressSelected: boolean;
}

interface AppState {
  appLoaded: boolean;
  userType: CustomerTypes | null;
  authenticationPage: AuthenticationPage;
}

export type { AppState };
