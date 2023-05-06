interface ModalState {
  open: boolean;
  body: JSX.Element | null;
  size?: "mini" | "tiny" | "small" | "large" | "fullscreen";
}

export type { ModalState };
