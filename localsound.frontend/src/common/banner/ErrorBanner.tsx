import { Alert } from "react-bootstrap";
import { Icon } from "semantic-ui-react";

interface Props {
  className?: string;
  children: string;
}

const ErrorBanner = ({ className, children }: Props) => {
  return (
    <Alert variant="danger" className={className + " text-center"}>
      <Icon name="warning sign" /> {children}
    </Alert>
  );
};

export default ErrorBanner;
