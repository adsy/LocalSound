import { useDispatch, useSelector } from "react-redux";
import { Modal } from "semantic-ui-react";
import { handleToggleModal } from "../../app/redux/actions/modalSlice";
import { State } from "../../app/model/redux/state";

const ModalContainer = () => {
  const dispatch = useDispatch();
  const modal = useSelector((state: State) => state.modal);
  const size = modal.size === undefined ? "mini" : modal.size;

  return (
    <Modal
      className="fade-in"
      closeIcon={{
        style: { top: "1.0535rem", right: "1rem" },
        color: "black",
        name: "close",
      }}
      open={modal.open}
      size={size}
      style={{ top: "unset", left: "unset", height: "unset" }}
      onClose={() =>
        dispatch(handleToggleModal({ open: !modal.open, body: null }))
      }
      dimmer={"inverted"}
    >
      <Modal.Content>{modal.body}</Modal.Content>
    </Modal>
  );
};

export default ModalContainer;
