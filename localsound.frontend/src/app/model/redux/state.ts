import { AppState } from "./appState";
import { ModalState } from "./modalState";
import { UserState } from "./userState";

interface State {
  app: AppState;
  modal: ModalState;
  user: UserState;
}

export type { State };
