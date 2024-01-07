import { useEffect, useMemo, useRef, useState } from "react";
import { GenreModel } from "../../../../../app/model/dto/genre.model";
import lodash from "lodash";
import agent from "../../../../../api/agent";
import Label from "../../../../../common/components/Label/Label";
import ErrorBanner from "../../../../../common/banner/ErrorBanner";

interface Props {
  selectedGenres: GenreModel[];
  setSelectedGenres: (genres: GenreModel[]) => void;
  placeholder: string;
}

const SearchGenreTypes = ({
  selectedGenres,
  setSelectedGenres,
  placeholder,
}: Props) => {
  const [genre, setGenre] = useState("");
  const [genreList, setGenreList] = useState<GenreModel[]>([]);
  const [error, setError] = useState<string | null>(null);
  const ref = useRef<() => void>();

  const onChange = async () => {
    try {
      setGenreList([]);
      if (genre.length > 0) {
        var result = await agent.Genre.searchGenre(genre);
        var genres = result.filter(
          (x) => !selectedGenres.find((z) => x.genreName === z.genreName)
        );
        setGenreList(genres.slice(0, 5));
      }
    } catch (e) {
      setError(
        "There was an error while searching, please refresh your page and try again."
      );
    }
  };

  const deleteSelectedGenre = (id: string) => {
    var genres = [...selectedGenres];

    setSelectedGenres(genres.filter((x) => x.genreId != id));
  };

  const addGenre = (genre: GenreModel) => {
    setGenre("");
    setGenreList([]);
    setSelectedGenres([...selectedGenres, genre]);
  };

  useEffect(() => {
    ref.current = onChange;
  }, [onChange]);

  const doCallbackWithDebounce = useMemo(() => {
    const callback = () => ref.current?.();
    return lodash.debounce(callback, 500);
  }, []);

  return (
    <div id="search-label-component">
      <div className="box d-flex flex-column justify-content-between">
        <div className="container">
          {selectedGenres.map((selectedGenre, index) => (
            <span key={index} className="badge-container">
              <Label
                label={selectedGenre.genreName}
                id={selectedGenre.genreId}
                deleteLabelItem={deleteSelectedGenre}
                showDeleteButton={true}
              />
            </span>
          ))}
        </div>

        {selectedGenres.length < 10 ? (
          <input
            className="input"
            placeholder={placeholder}
            value={genre}
            onChange={(e) => {
              doCallbackWithDebounce();
              setGenre(e.target.value);
            }}
          />
        ) : null}
      </div>
      <div className="positive-relative">
        {genreList.length > 0 ? (
          <div className="dropdown">
            {genreList.map((genre, index) => {
              return (
                <div
                  key={index}
                  onClick={() => addGenre(genre)}
                  className="dropdown-option"
                >
                  {genre.genreName}
                </div>
              );
            })}
          </div>
        ) : null}
      </div>
      {error ? (
        <ErrorBanner className="mt-2 text-center">{error}</ErrorBanner>
      ) : null}
    </div>
  );
};

export default SearchGenreTypes;
