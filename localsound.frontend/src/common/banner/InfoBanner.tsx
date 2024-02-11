import { Alert } from "react-bootstrap";
import { Icon } from "semantic-ui-react";

interface Props {
  className?: string;
  children: string | JSX.Element;
  closable?: boolean;
  closeFunc?: () => void;
}

const InfoBanner = ({
  className,
  children,
  closeFunc,
  closable = false,
}: Props) => {
  return (
    <Alert variant="info" className={className + " text-center"}>
      {closable ? (
        <Icon
          name="close"
          size="small"
          className="close-icon"
          onClick={async () => {
            if (closeFunc) {
              await closeFunc();
            }
          }}
        />
      ) : null}
      {children}
    </Alert>
  );
};

export default InfoBanner;
