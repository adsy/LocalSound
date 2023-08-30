import { useEffect, useMemo, useRef, useState } from "react";
import { GenreModel } from "../../../../../app/model/dto/genre.model";
import lodash from "lodash";
import agent from "../../../../../api/agent";
import { Badge } from "react-bootstrap";
import GenreTypeLabel from "./GenreTypeLabel";

const SearchGenreTypes = () => {
  const [genre, setGenre] = useState("");
  const [genreList, setGenreList] = useState<GenreModel[]>([]);
  const [selectedGenres, setSelectedGenres] = useState<GenreModel[]>([]);
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
      //TODO: Do something with error
    }
  };

  const deleteSelectedGenre = (genre: GenreModel) => {
    var genres = [...selectedGenres];

    setSelectedGenres(genres.filter((x) => x.genreId != genre.genreId));
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
    <div id="search-genre">
      <div className="genre-box d-flex flex-column justify-content-between">
        <div className="genre-container">
          {selectedGenres.map((selectedGenre, index) => (
            <GenreTypeLabel
              genre={selectedGenre}
              index={index}
              deleteSelectedGenre={deleteSelectedGenre}
            />
          ))}
        </div>

        <input
          className="genre-input"
          placeholder="Search for a genre to add to your profile"
          value={genre}
          onChange={(e) => {
            doCallbackWithDebounce();
            setGenre(e.target.value);
          }}
        />
      </div>
      <div className="positive-relative">
        {genreList.length > 0 ? (
          <div className="genre-dropdown">
            {genreList.map((genre, index) => {
              return (
                <div
                  key={index}
                  onClick={() => addGenre(genre)}
                  className="genre-dropdown-option"
                >
                  {genre.genreName}
                </div>
              );
            })}
          </div>
        ) : null}
      </div>
    </div>
  );
};

export default SearchGenreTypes;
