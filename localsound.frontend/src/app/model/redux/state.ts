import { AppState } from "./appState";
import { ModalState } from "./modalState";
import { PageOperationState } from "./pageOperationState";
import { PlayerState } from "./playerState";
import { ProfileState } from "./profileState";
import { UserState } from "./userState";

interface State {
  app: AppState;
  modal: ModalState;
  user: UserState;
  player: PlayerState;
  pageOperation: PageOperationState;
  profile: ProfileState;
}

export type { State };
