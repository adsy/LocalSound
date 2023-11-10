import { useDispatch, useSelector } from "react-redux";
import { Modal, TransitionablePortal } from "semantic-ui-react";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import { State } from "../../app/model/redux/state";
import { useEffect } from "react";

const ModalContainer = () => {
  const dispatch = useDispatch();
  const modal = useSelector((state: State) => state.modal);

  useEffect(() => {
    if (!modal.open) {
      document.body.classList.remove("modal-fade-in");
    }
  }, [modal.open]);

  return (
    <div>
      <TransitionablePortal
        open={modal.open}
        onOpen={() =>
          setTimeout(() => document.body.classList.add("modal-fade-in"), 0)
        }
        transition={{ animation: "fly up", duration: 800 }}
      >
        <Modal
          closeOnDimmerClick={false}
          open={true}
          onClose={() => {
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
