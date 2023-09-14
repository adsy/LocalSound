import { Alert } from "react-bootstrap";

interface Props {
  className?: string;
  children: string;
}

const ErrorBanner = ({ className, children }: Props) => {
  return (
    <Alert variant="danger" className={className}>
      {children}
    </Alert>
  );
};

export default ErrorBanner;
