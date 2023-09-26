import {
  SyntheticEvent,
  useEffect,
  useLayoutEffect,
  useRef,
  useState,
} from "react";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import WaveFormContainer from "./TrackContainer";
import PlayButton from "./PlayButton";
import { Button } from "react-bootstrap";
import Label from "../Label/Label";
import { useDispatch, useSelector } from "react-redux";
import { handleSetPlayerSong } from "../../../app/redux/actions/playerSlice";
import { State } from "../../app/model/redux/state";

const MusicPlayer = () => {
  const player = useSelector((state: State) => state.player);
  const [time, setTime] = useState<any>(null);
  const [totalTime, setTotalTime] = useState<string | null>(null);
  const waveformRef = useRef<HTMLAudioElement>(null);

  useEffect(() => {
    if (waveformRef.current) {
      waveformRef.current.src = player.trackUrl!;

      if (player.playing) {
        waveformRef.current.play();
      } else {
        waveformRef.current.pause();
      }
    }
  }, [player]);

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
  return (
    <>
      container
      <div id="waveform" />
      <audio
        id="music"
        preload="all"
        ref={waveformRef}
        onTimeUpdate={(e) => updateTime(e)}
      ></audio>
      {/* <Button onClick={() => test()}>Test</Button> */}
      {time && totalTime ? (
        <>
          {time}\{totalTime}
        </>
      ) : null}
    </>
  );
};

export default MusicPlayer;
