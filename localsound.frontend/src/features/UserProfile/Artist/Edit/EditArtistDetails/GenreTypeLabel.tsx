import { Badge } from "react-bootstrap";
import { GenreModel } from "../../../../../app/model/dto/genre.model";

interface Props {
  genre: GenreModel;
  index: number;
  deleteSelectedGenre: (genre: GenreModel) => void;
}

const GenreTypeLabel = ({ genre, index, deleteSelectedGenre }: Props) => {
  return (
    <Badge key={index} className="purple-badge mr-1">
      <div className="d-flex flex-row">
        <div className="badge-genre-name">{genre.genreName}</div>
        <div
          className="ml-1 badge-delete-btn"
          onClick={() => deleteSelectedGenre(genre)}
        >
          X
        </div>
      </div>
    </Badge>
  );
};

export default GenreTypeLabel;
