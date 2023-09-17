import { Alert } from "react-bootstrap";
import { Icon } from "semantic-ui-react";

interface Props {
  className?: string;
  children: string;
}

const SuccessBanner = ({ className, children }: Props) => {
  return (
    <Alert variant="success" className={className + " text-center"}>
      <Icon name="check" /> {children}
    </Alert>
  );
};

export default SuccessBanner;
