import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import TrackContainer from "./TrackContainer";
import PlayButton from "./PlayButton";
import Label from "../Label/Label";
import { useDispatch, useSelector } from "react-redux";
import {
  handlePauseSong,
  handlePlaySong,
  handleSetPlayerSong,
} from "../../../app/redux/actions/playerSlice";
import { State } from "../../../app/model/redux/state";

interface Props {
  track: ArtistTrackUploadModel;
}

const Track = ({ track }: Props) => {
  const player = useSelector((state: State) => state.player);
  const dispatch = useDispatch();

  const playSong = () => {
    if (player.playing && player.trackId === track.artistTrackUploadId) {
      dispatch(handlePauseSong());
    } else if (
      !player.playing &&
      player.trackId === track.artistTrackUploadId
    ) {
      dispatch(handlePlaySong());
    } else {
      dispatch(
        handleSetPlayerSong({
          trackId: track.artistTrackUploadId,
          trackUrl: track.trackUrl,
        })
      );
    }
  };

  return (
    <div id="waveform" className="d-flex flex-column mt-3">
      <h3 className="mb-0">{track.trackName}</h3>
      <TrackContainer>
        <PlayButton
          handlePlay={playSong}
          playing={
            track.artistTrackUploadId === player.trackId && player.playing
          }
        />
      </TrackContainer>

      <div>
        {track.genres.map((genre) => (
          <Label id={genre.genreId} label={genre.genreName} />
        ))}
      </div>
      <p>{track.trackDescription}</p>
    </div>
  );
};

export default Track;
