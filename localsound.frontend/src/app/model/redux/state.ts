import { AppState } from "./appState";
import { ModalState } from "./modalState";
import { PageOperationState } from "./pageOperationState";
import { PlayerState } from "./playerState";
import { PageDataState } from "./pageDataState";
import { UserState } from "./userState";
import { NotificationState } from "./notificationState";

interface State {
  app: AppState;
  modal: ModalState;
  user: UserState;
  player: PlayerState;
  pageOperation: PageOperationState;
  pageData: PageDataState;
  notifications: NotificationState;
}

export type { State };
