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
// @ts-ignore
import CanvasJSReact from "@canvasjs/react-charts";
import { useLayoutEffect, useState } from "react";

interface Props {
  track: ArtistTrackUploadModel;
  artistDetails: UserModel;
}

const Track = ({ track, artistDetails }: Props) => {
  const player = useSelector((state: State) => state.player);
  const dispatch = useDispatch();
  const [dps, setDps] = useState<{ [key: string]: any }>(null);

  useLayoutEffect(() => {
    (async () => {
      await fetch(track.waveformUrl)
        .then((data) => {
          return data.json();
        })
        .then((json) => {
          const options = {
            height: 100,
            title: {
              dockInsidePlotArea: true,
              verticalAlign: "center",
            },
            axisX: {
              tickLength: 0,
              lineThickness: 0,
              labelFontSize: 0,
            },
            axisY: {
              tickLength: 0,
              lineThickness: 0,
              gridThickness: 0,
              labelFontSize: 0,
            },
            data: [
              {
                type: "rangeArea",
                toolTipContent: null,
                highlightEnabled: false,
                color: "#6d69fa",
                dataPoints: json.data,
              },
            ],
          };

          setDps(options);
        });
    })();
  }, []);

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

  var CanvasJS = CanvasJSReact.CanvasJS;
  var CanvasJSChart = CanvasJSReact.CanvasJSChart;

  return (
    <div id="waveform" className="d-flex flex-column mt-3 w-100">
      <div className="d-flex flex-column">
        <div className="d-flex flex-row w-100">
          <Image size="small" rounded src={track.trackImageUrl} />
          <div className="d-flex flex-column justify-content-between">
            <div className="d-flex flex-column">
              <div>
                <h3 className="mb-0">{track.trackName}</h3>
              </div>
            </div>
            <div style={{ height: "50px" }}>
              {dps ? <CanvasJSChart options={dps} /> : null}
            </div>

            <TrackContainer>
              <PlayButton
                handlePlay={playSong}
                playing={
                  track.artistTrackUploadId === player.trackId && player.playing
                }
              />
            </TrackContainer>
            <div>
              {track.genres.map((genre, index) => (
                <Label key={index} id={genre.genreId} label={genre.genreName} />
              ))}
            </div>
          </div>
        </div>
      </div>

      <p>{track.trackDescription}</p>
    </div>
  );
};

export default Track;
