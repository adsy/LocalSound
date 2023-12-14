import { Alert } from "react-bootstrap";

interface Props {
  className?: string;
  children: string | JSX.Element;
}

const InfoBanner = ({ className, children }: Props) => {
  return (
    <Alert variant="info" className={className + " text-center"}>
      {children}
    </Alert>
  );
};

export default InfoBanner;
