import { createSlice } from "@reduxjs/toolkit";
import { ModalState } from "../../model/redux/modalState";

const initialState: ModalState = {
  open: false,
  body: null,
  size: undefined,
};

export const modalSlice = createSlice({
  name: "modal",
  initialState,
  reducers: {
    handleToggleModal: (state = initialState, action) => {
      state.open = action.payload.open;
      state.body = action.payload.body;

      if (action.payload.size) {
        state.size = action.payload.size;
      } else {
        state.size = "mini";
      }
    },
  },
});

export const { handleToggleModal } = modalSlice.actions;

export default modalSlice.reducer;
