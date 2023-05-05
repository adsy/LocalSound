import React from "react";
import { Spinner } from "react-bootstrap";
import { Dimmer } from "semantic-ui-react";

interface Props {
  inverted?: boolean;
  content?: string;
}

const LoadingComponent = ({ inverted = true, content = "" }: Props) => {
  return (
    <span className="fade-in">
      <Dimmer active={true} inverted={inverted} className="animate">
        <Spinner animation={"border"} variant="primary" />
        {content.length ? (
          <div className="text-center spinner-text-spacing">
            <h3 className="black-text">{content}</h3>
          </div>
        ) : null}
      </Dimmer>
    </span>
  );
};

export default LoadingComponent;
