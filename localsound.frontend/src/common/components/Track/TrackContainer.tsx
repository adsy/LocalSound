import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

const TrackContainer = ({ children }: Props) => {
  return (
    <div className="playbutton-container position-absolute">{children}</div>
  );
};

export default TrackContainer;
