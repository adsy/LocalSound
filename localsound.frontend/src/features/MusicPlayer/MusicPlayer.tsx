import { SyntheticEvent, useLayoutEffect, useRef, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { Icon } from "semantic-ui-react";
import { Container } from "react-bootstrap";
import {
  handlePauseSong,
  handlePlaySong,
} from "../../app/redux/actions/playerSlice";
import WaveForm from "./WaveFrom";
import { SingletonFactory } from "../../common/waveformGenerator/waveformGenerator";

const MusicPlayer = () => {
  const player = useSelector((state: State) => state.player);
  const [time, setTime] = useState<any>(null);
  const [totalTime, setTotalTime] = useState<string | null>(null);
  const [currentTrack, setCurrentTrack] = useState<string | null>(null);
  const waveformRef = useRef<HTMLAudioElement>(null);
  const seekerRef = useRef<HTMLInputElement>(null);
  const [mediaElementSource, setMediaElementSource] =
    useState<MediaElementAudioSourceNode>();
  const dispatch = useDispatch();

  var singleton = SingletonFactory.getInstance();

  useLayoutEffect(() => {
    if (waveformRef.current) {
      singleton.audioElementRef = waveformRef;
      waveformRef!.current!.crossOrigin = "anonymous";
      if (currentTrack !== player.trackId) {
        waveformRef.current.src = player.trackUrl!;
        setCurrentTrack(player.trackId);
        seekerRef!.current!.value = "0";
        if (!mediaElementSource) {
          audioAnalyzer();
        }
      }

      if (player.playing) {
        waveformRef.current.play();
      } else {
        waveformRef.current.pause();
      }
    }
  }, [player]);

  const audioAnalyzer = () => {
    // create a new AudioContext
    const audioCtx = new window.AudioContext();
    // create an analyzer node with a buffer size of 2048
    const analyzer = audioCtx.createAnalyser();
    analyzer.fftSize = 2048;

    const bufferLength = analyzer.frequencyBinCount;
    const dataArray = new Uint8Array(bufferLength);

    if (!mediaElementSource) {
      const source = audioCtx.createMediaElementSource(waveformRef!.current!);
      source.connect(analyzer);
      source.connect(audioCtx.destination);
      setMediaElementSource(source);
    }
    // source.disconnect();
    // set the analyzerData state with the analyzer, bufferLength, and dataArray
    // setAnalyzerData({ analyzer, bufferLength, dataArray });
    singleton.analyzerData = { analyzer, bufferLength, dataArray };
  };

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
    if (waveformRef.current && seekerRef.current) {
      var time =
        waveformRef.current?.currentTime && waveformRef.current.currentTime > 0
          ? waveformRef.current.currentTime
          : 0;

      seekerRef.current.value = `${
        (waveformRef.current.currentTime / waveformRef.current.duration) * 10000
      }`;

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

      if (waveformRef.current.currentTime == waveformRef.current.duration) {
        dispatch(handlePauseSong());
      }
    }
  };

  const updateCurrentTime = (e: React.ChangeEvent<HTMLInputElement>) => {
    var percentage = Number.parseInt(e.target.value) / 10000;
    var pointOfSong = waveformRef.current?.duration! * percentage;
    waveformRef!.current!.currentTime = pointOfSong;
  };

  return (
    <div id="music-player" className="fade-in">
      <Container className="px-3 d-flex flex-column align-items-between h-100">
        <div className="mt-3 player-container mb-2">
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
        </div>
      </Container>
    </div>
  );
};

export default MusicPlayer;
