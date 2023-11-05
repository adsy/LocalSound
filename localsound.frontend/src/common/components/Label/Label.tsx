import { Badge } from "react-bootstrap";

interface Props {
  label: string;
  id: string;
  deleteLabelItem?: (id: string) => void;
  showDeleteButton?: boolean;
  color?: string;
}

const Label = ({
  label,
  id,
  deleteLabelItem,
  showDeleteButton = false,
  color = "black-badge",
}: Props) => {
  return (
    <Badge className={`${color}`}>
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
