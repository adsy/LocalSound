import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

const WaveFormContainer = ({ children }: Props) => {
  return <div className="waveform-container">{children}</div>;
};

export default WaveFormContainer;
