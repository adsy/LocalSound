import { createSlice } from "@reduxjs/toolkit";
import { ActionState } from "../../model/redux/actionState";

const initialState: ActionState = {
  isDeleting: false,
};
export const actionSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    handleSetIsDeleting: (state = initialState, action) => {
      state.isDeleting = action.payload;
    },
  },
});

export const { handleSetIsDeleting } = actionSlice.actions;

export default actionSlice.reducer;
