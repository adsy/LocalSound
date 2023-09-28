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
import { UserModel } from "../../../app/model/dto/user.model";
import { Image } from "semantic-ui-react";
import { useLayoutEffect, useState } from "react";
// import Waveform from "./Waveform";
import { BargraphData } from "../../../app/model/dto/bargraph-data-model";

interface Props {
  track: ArtistTrackUploadModel;
  artistDetails: UserModel;
}

const Track = ({ track, artistDetails }: Props) => {
  const player = useSelector((state: State) => state.player);
  const dispatch = useDispatch();
  const [dps, setDps] = useState<BargraphData[] | null>(null);

  // useLayoutEffect(() => {
  //   (async () => {
  //     await fetch(track.waveformUrl)
  //       .then((data) => {
  //         return data.json();
  //       })
  //       .then((json) => {
  //         setDps(json.data);
  //       });
  //   })();
  // }, []);

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
          artistProfile: artistDetails.profileUrl,
          trackName: track.trackName,
        })
      );
    }
  };

  return (
    <div id="track">
      <div className="d-flex flex-column">
        <div className="d-flex flex-row w-100 align-items-center">
          <div className="d-flex flex-column justify-content-between">
            <div className="d-flex flex-column">
              <div>
                <h3 className="mb-0">{track.trackName}</h3>
              </div>
            </div>

            <div>
              {track.genres.map((genre, index) => (
                <Label key={index} id={genre.genreId} label={genre.genreName} />
              ))}
            </div>
          </div>
        </div>
      </div>
      <div className="d-flex flex-row w-100 align-items-center position-relative">
        <Image size="small" rounded src={track.trackImageUrl} />

        <TrackContainer>
          <PlayButton
            handlePlay={playSong}
            playing={
              track.artistTrackUploadId === player.trackId && player.playing
            }
          />
        </TrackContainer>
      </div>
    </div>
  );
};

export default Track;
