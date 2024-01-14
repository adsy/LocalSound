import spinner from "../../assets/Equalizer.svg";

interface Props {
  inverted?: boolean;
  content?: string;
  height?: number;
  width?: number;
  withContainer?: boolean;
  containerClass?: string;
}

const InPageLoadingComponent = ({
  inverted = true,
  content,
  height = 50,
  width = 50,
  withContainer = false,
  containerClass,
}: Props) => {
  return (
    <div
      id="loading-component"
      className={`fade-in d-flex flex-column justify-content-center ${
        withContainer ? "component-container" : null
      } ${containerClass}`}
    >
      <img
        src={spinner}
        style={{ height: height, width: width }}
        className="align-self-center"
        alt="loading-icon"
      />
      {content && (
        <div className="text-center mt-2 loading-text">
          <h3>{content}</h3>
        </div>
      )}
    </div>
  );
};

export default InPageLoadingComponent;
