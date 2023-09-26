import { SyntheticEvent, useLayoutEffect, useRef, useState } from "react";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import WaveFormContainer from "./WaveFormContainer";
import PlayButton from "./PlayButton";
import { Button } from "react-bootstrap";
import Label from "../Label/Label";

interface Props {
  track: ArtistTrackUploadModel;
}

const Track = ({ track }: Props) => {
  const [playing, setPlaying] = useState(false);
  const [time, setTime] = useState<any>(null);
  const [totalTime, setTotalTime] = useState<string | null>(null);
  const waveformRef = useRef<HTMLAudioElement>(null);

  useLayoutEffect(() => {}, [playing]);

  const getTotalTime = () => {
    var time = waveformRef!.current!.duration;
    var totalHours = Math.trunc(time / 3600);
    var hoursRemainder = (time % 3600) / 3600;

    var minutes = hoursRemainder * 60;
    var totalMinutes = Math.trunc(minutes);
    var totalMinutesStr = totalMinutes.toString();
    var minutesRemainder = minutes % totalMinutes;
    if (totalMinutesStr.length < 2) {
      totalMinutesStr = "0" + totalMinutesStr;
    }

    var seconds = Math.trunc(minutesRemainder * 60);
    var secondsString = seconds.toString();
    if (secondsString.length < 2) {
      secondsString = "0" + secondsString;
    }

    setTotalTime(`${totalHours}:${totalMinutesStr}:${secondsString}`);
  };

  const updateTime = (event: SyntheticEvent<HTMLAudioElement, Event>) => {
    var time =
      waveformRef.current?.currentTime && waveformRef.current.currentTime > 0
        ? waveformRef.current.currentTime
        : 0;

    time = Math.trunc(time);

    var mins = 0;
    if (time > 60) {
      mins = Math.trunc(time / 60);
    }

    if (mins > 0) {
      time = time - mins * 60;
    } else {
      mins = 0;
    }

    var secondsText = time.toString();
    var minsText = mins.toString();

    if (secondsText.length < 2) {
      secondsText = "0" + secondsText;
    }
    if (minsText.length < 2) {
      minsText = "0" + minsText;
    }
    getTotalTime();
    setTime(`00:${minsText}:${secondsText}`);
  };

  const handlePlay = async () => {
    if (waveformRef.current) {
      if (!waveformRef.current.src) {
        waveformRef.current.src = track.trackUrl;
      }

      if (playing) {
        waveformRef.current.pause();
      } else {
        waveformRef.current.play();
      }
      setPlaying(!playing);
    }
  };

  const test = () => {
    if (waveformRef.current) {
      waveformRef.current.currentTime = 30;
    }
  };

  return (
    <div id="waveform" className="d-flex flex-column mt-3">
      <h3 className="mb-0">{track.trackName}</h3>
      <WaveFormContainer>
        <PlayButton handlePlay={handlePlay} playing={playing} />
        <div id="waveform" />
        <audio
          id="music"
          preload="all"
          ref={waveformRef}
          onTimeUpdate={(e) => updateTime(e)}
        ></audio>
      </WaveFormContainer>
      {/* <Button onClick={() => test()}>Test</Button> */}
      {/* {time && totalTime ? (
        <>
          {time}\{totalTime}
        </>
      ) : null} */}
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
