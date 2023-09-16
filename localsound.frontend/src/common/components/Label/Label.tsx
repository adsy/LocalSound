import { Badge } from "react-bootstrap";

interface Props {
  label: string;
  id: string;
  deleteLabelItem?: (id: string) => void;
  showDeleteButton?: boolean;
}

const Label = ({
  label,
  id,
  deleteLabelItem,
  showDeleteButton = false,
}: Props) => {
  return (
    <Badge className="purple-badge mr-1">
      <div className="d-flex flex-row">
        <div className={`badge-name ${showDeleteButton ? "short" : "long"}`}>
          {label}
        </div>
        {showDeleteButton ? (
          <div
            className="ml-1 badge-delete-btn"
            onClick={() => {
              if (deleteLabelItem) deleteLabelItem(id);
            }}
          >
            X
          </div>
        ) : null}
      </div>
    </Badge>
  );
};

export default Label;
