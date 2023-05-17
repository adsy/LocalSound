import { useDispatch, useSelector } from "react-redux";
import { Button, Modal, TransitionablePortal } from "semantic-ui-react";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import { State } from "../../app/model/redux/state";

const ModalContainer = () => {
  const dispatch = useDispatch();
  const modal = useSelector((state: State) => state.modal);
  const size = modal.size === undefined ? "mini" : modal.size;

  return (
    <div>
      <TransitionablePortal
        open={modal.open}
        onOpen={() =>
          setTimeout(() => document.body.classList.add("modal-fade-in"), 0)
        }
        transition={{ animation: "fly up", duration: 500 }}
      >
        <Modal
          open={true}
          onClose={() => {
            document.body.classList.remove("modal-fade-in");
            dispatch(handleToggleModal({ open: false }));
          }}
          style={{ top: "unset", left: "unset", height: "unset" }}
          closeIcon={{
            style: { top: "1.0535rem", right: "1rem" },
            color: "black",
            name: "close",
          }}
          size={modal.size}
        >
          {modal.body?.type?.name ? (
            <Modal.Content>{modal.body}</Modal.Content>
          ) : (
            <Modal.Content></Modal.Content>
          )}
        </Modal>
      </TransitionablePortal>
    </div>
  );
};

export default ModalContainer;
