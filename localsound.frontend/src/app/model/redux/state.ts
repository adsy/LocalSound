import { ActionState } from "./actionState";
import { AppState } from "./appState";
import { ModalState } from "./modalState";
import { PageOperationState } from "./pageOperationState";
import { PlayerState } from "./playerState";
import { UserState } from "./userState";

interface State {
  app: AppState;
  modal: ModalState;
  user: UserState;
  player: PlayerState;
  action: ActionState;
  pageOperation: PageOperationState;
}

export type { State };
