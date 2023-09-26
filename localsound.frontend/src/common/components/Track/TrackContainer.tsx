import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

const TrackContainer = ({ children }: Props) => {
  return <div className="waveform-container">{children}</div>;
};

export default TrackContainer;
