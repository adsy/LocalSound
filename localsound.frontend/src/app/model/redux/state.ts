import { AppState } from "./appState";
import { ModalState } from "./modalState";
import { PageOperationState } from "./pageOperationState";
import { PlayerState } from "./playerState";
import { PageDataState } from "./pageDataState";
import { UserState } from "./userState";

interface State {
  app: AppState;
  modal: ModalState;
  user: UserState;
  player: PlayerState;
  pageOperation: PageOperationState;
  profile: PageDataState;
}

export type { State };
