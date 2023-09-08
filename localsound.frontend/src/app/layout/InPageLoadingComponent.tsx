import React from "react";
import spinner from "../../assets/Equalizer.svg";
import logo from "../../assets/logo4.png";

interface Props {
  inverted?: boolean;
  content?: string;
  height?: number;
  width?: number;
}

const InPageLoadingComponent = ({
  inverted = true,
  content,
  height = 50,
  width = 50,
}: Props) => {
  return (
    <span className="fade-in d-flex flex-column justify-content-center">
      <img
        src={spinner}
        style={{ height: height, width: width }}
        className="align-self-center"
        alt="loading-icon"
      />
      {content && (
        <div className="text-center mt-2">
          <h3>{content}</h3>
        </div>
      )}
    </span>
  );
};

export default InPageLoadingComponent;
