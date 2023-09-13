import { Alert } from "react-bootstrap";

interface Props {
  text: string;
  className: string;
}

const ErrorBanner = ({ text, className }: Props) => {
  return (
    <Alert variant="danger" className={className}>
      {text}
    </Alert>
  );
};

export default ErrorBanner;
