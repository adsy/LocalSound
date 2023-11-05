import { ActionState } from "./actionState";
import { AppState } from "./appState";
import { ModalState } from "./modalState";
import { PlayerState } from "./playerState";
import { UserState } from "./userState";

interface State {
  app: AppState;
  modal: ModalState;
  user: UserState;
  player: PlayerState;
  action: ActionState;
}

export type { State };
