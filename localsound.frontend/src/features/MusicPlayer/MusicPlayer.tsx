import { SyntheticEvent, useLayoutEffect, useRef, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { Icon } from "semantic-ui-react";
import { Container } from "react-bootstrap";
import {
  handlePauseSong,
  handlePlaySong,
} from "../../app/redux/actions/playerSlice";

const MusicPlayer = () => {
  const player = useSelector((state: State) => state.player);
  const [time, setTime] = useState<any>(null);
  const [totalTime, setTotalTime] = useState<string | null>(null);
  const [currentTrack, setCurrentTrack] = useState<string | null>(null);
  const waveformRef = useRef<HTMLAudioElement>(null);
  const seekerRef = useRef<HTMLInputElement>(null);
  const dispatch = useDispatch();

  useLayoutEffect(() => {
    if (waveformRef.current) {
      if (currentTrack !== player.trackId) {
        waveformRef.current.src = player.trackUrl!;
        setCurrentTrack(player.trackId);
        seekerRef!.current!.value = "0";
      }

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

  const updateCurrentTime = (e: React.ChangeEvent<HTMLInputElement>) => {
    var percentage = Number.parseInt(e.target.value) / 10000;
    var pointOfSong = waveformRef.current?.duration! * percentage;
    waveformRef!.current!.currentTime = pointOfSong;
  };

  return (
    <div id="music-player" className="fade-in">
      <Container className="px-3 player-container">
        <audio
          id="music"
          preload="all"
          ref={waveformRef}
          onTimeUpdate={(e) => updateTime(e)}
        ></audio>
        <div className="pr-3">
          {!player.playing ? (
            <Icon
              className="audio-button"
              name="play"
              size="large"
              color="grey"
              onClick={() => {
                dispatch(handlePlaySong());
              }}
            />
          ) : (
            <Icon
              className="audio-button"
              name="pause"
              size="large"
              color="grey"
              onClick={() => {
                dispatch(handlePauseSong());
              }}
            />
          )}
        </div>
        <h5 className="m-0 pr-3">{time}</h5>
        <input
          ref={seekerRef}
          type="range"
          className="seek-slider"
          max="10000"
          onChange={(e) => {
            updateCurrentTime(e);
          }}
        ></input>
        {/* <h3 className="m-0">{player.trackName}</h3> */}
        {time && totalTime ? <h5 className="m-0 pl-3">{totalTime}</h5> : null}
      </Container>
    </div>
  );
};

export default MusicPlayer;
