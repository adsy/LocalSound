import { Badge } from "react-bootstrap";
import { GenreModel } from "../../../../../app/model/dto/genre.model";

interface Props {
  genre: GenreModel;
  deleteSelectedGenre?: (genre: GenreModel) => void;
  showDeleteButton?: boolean;
}

const GenreTypeLabel = ({
  genre,
  deleteSelectedGenre,
  showDeleteButton = false,
}: Props) => {
  return (
    <Badge className="purple-badge mr-1">
      <div className="d-flex flex-row">
        <div
          className={`badge-genre-name ${showDeleteButton ? "short" : "long"}`}
        >
          {genre.genreName}
        </div>
        {showDeleteButton ? (
          <div
            className="ml-1 badge-delete-btn"
            onClick={() => {
              if (deleteSelectedGenre) deleteSelectedGenre(genre);
            }}
          >
            X
          </div>
        ) : null}
      </div>
    </Badge>
  );
};

export default GenreTypeLabel;
