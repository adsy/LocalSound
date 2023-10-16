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
import { useEffect, useState } from "react";
// import Waveform from "./Waveform";
import { BargraphData } from "../../../app/model/dto/bargraph-data-model";
import {
  SingletonClass,
  SingletonFactory,
} from "../../waveformGenerator/waveformGenerator";
import WaveForm from "../../../features/MusicPlayer/Waveform";

interface Props {
  track: ArtistTrackUploadModel;
  artistDetails: UserModel;
}

const Track = ({ track, artistDetails }: Props) => {
  const player = useSelector((state: State) => state.player);
  const dispatch = useDispatch();
  const [dps, setDps] = useState<BargraphData[] | null>(null);
  const [singleton, setSingleton] = useState<SingletonClass>(
    SingletonFactory.getInstance()
  );
  const [analyzerData, setAnalyzerData] = useState();

  useEffect(() => {
    if (player.trackId === track.artistTrackUploadId) {
      setAnalyzerData(singleton.analyzerData);
    }
  }, [player.trackId, player.trackName, player.playing]);

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
          artistName: artistDetails.name,
          trackImage: track.trackImageUrl,
          duration: track.duration,
        })
      );
    }
  };

  // console.log(singleton);

  return (
    <div id="track" className="mb-4">
      <div className="d-flex flex-row w-100">
        <Image size="small" src={track.trackImageUrl} className="mr-3" />

        <div className="d-flex flex-column w-100">
          <div className="d-flex flex-row justify-content-between">
            <div className="d-flex flex-row">
              <TrackContainer>
                <PlayButton
                  handlePlay={playSong}
                  playing={
                    track.artistTrackUploadId === player.trackId &&
                    player.playing
                  }
                />
              </TrackContainer>
              <div className="d-flex flex-column ml-2">
                <p className="artist-name mb-0">{artistDetails.name}</p>
                <p className="mb-0 track-name">{track.trackName}</p>
              </div>
            </div>

            <div>
              {track.genres.map((genre, index) => (
                <Label key={index} id={genre.genreId} label={genre.genreName} />
              ))}
            </div>
          </div>

          <div className="w-100 h-100 d-flex flex-column align-items-center">
            <div className="line w-100 h-100 position-relative">
              {track.artistTrackUploadId === player.trackId && analyzerData && (
                <div>
                  <WaveForm analyzerData={analyzerData} />
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Track;
