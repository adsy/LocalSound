import { useEffect, useMemo, useRef, useState } from "react";
import { GenreModel } from "../../../../../app/model/dto/genre.model";
import lodash from "lodash";
import agent from "../../../../../api/agent";
import { Badge } from "react-bootstrap";

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
    <>
      <div className="genre-box">
        {selectedGenres.map((selectedGenre, index) => (
          <Badge key={index} className="purple-badge mr-1">
            {selectedGenre.genreName}
          </Badge>
        ))}
      </div>
      <span>Search for genres to add to your profile</span>
      <input
        value={genre}
        onChange={(e) => {
          doCallbackWithDebounce();
          setGenre(e.target.value);
        }}
      />
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
    </>
  );
};

export default SearchGenreTypes;
