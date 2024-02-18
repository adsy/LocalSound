import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import { Icon, Image } from "semantic-ui-react";
import { Container } from "react-bootstrap";
import {
  handlePauseSong,
  handlePlaySong,
  handleSetPlayerSong,
  handleSetTrackList,
} from "../../app/redux/actions/playerSlice";
import { SingletonFactory } from "../../common/appSingleton/appSingleton";
import agent from "../../api/agent";
import InPageLoadingComponent from "../../app/layout/InPageLoadingComponent";
import { useHistory } from "react-router-dom";
import PlaceholderImg from "../../assets/placeholder.png";

const MusicPlayer = () => {
  const history = useHistory();
  const player = useSelector((state: State) => state.player);
  const [time, setTime] = useState<any>(null);
  const [totalTime, setTotalTime] = useState<string | null>(null);
  const [currentTrack, setCurrentTrack] = useState<number | null>(null);
  const waveformRef = useRef<HTMLMediaElement>(null);
  const seekerRef = useRef<HTMLInputElement>(null);
  const volumeRef = useRef<HTMLInputElement>(null);
  const [volumeMuted, setVolumeMuted] = useState(false);
  const [volume, setVolume] = useState(1);
  const [loadingMore, setLoadingMore] = useState(false);
  const [mediaElementSource, setMediaElementSource] =
    useState<MediaElementAudioSourceNode>();
  const [audioContext] = useState<AudioContext>(new window.AudioContext());
  const dispatch = useDispatch();

  var singleton = SingletonFactory.getInstance();

  useEffect(() => {
    if (volumeMuted) {
      if (waveformRef.current) {
        waveformRef.current.volume = 0;
        volumeRef.current!.value = "0";
      }
    } else {
      if (waveformRef.current) {
        waveformRef.current.volume = volume;
        volumeRef.current!.value = `${volume * 10000}`;
      }
    }
  }, [volumeMuted]);

  useLayoutEffect(() => {
    if (volumeRef?.current) {
      volumeRef.current.value = "10000";
    }

    if (waveformRef.current) {
      singleton.audioElementRef = waveformRef;
      waveformRef!.current!.crossOrigin = "anonymous";

      if (currentTrack !== player.currentSong?.trackId) {
        // disconnect the source if it exists when swapping tracks
        mediaElementSource?.disconnect();

        // get track data
        getTotalTime();
        setCurrentTrack(player.currentSong?.trackId!);

        // set ref values
        seekerRef!.current!.value = "0";
        waveformRef.current.src = player.currentSong?.trackUrl!;

        // create audio analyzer
        audioAnalyzer();

        // play the song
        setTimeout(() => {
          waveformRef!.current!.play();
          dispatch(handlePlaySong());
        }, 0);
      } else if (player.currentSong?.playing) {
        // create audio analyzer
        audioAnalyzer();
        waveformRef.current.play();
      } else if (!player.currentSong?.playing) {
        waveformRef.current.pause();
        mediaElementSource?.disconnect();
      }
    }
  }, [player.currentSong]);

  const audioAnalyzer = () => {
    // create an analyzer node with a buffer size of 2048
    var analyzer = audioContext.createAnalyser();
    analyzer.fftSize = 2048;

    const bufferLength = analyzer.frequencyBinCount;
    const dataArray = new Uint8Array(bufferLength);

    if (!mediaElementSource) {
      const source = audioContext.createMediaElementSource(
        waveformRef!.current!
      );
      source.connect(analyzer);
      source.connect(audioContext.destination);
      setMediaElementSource(source);
    } else {
      mediaElementSource!.connect(analyzer);
      mediaElementSource!.connect(audioContext.destination);
    }

    var trackid = player.currentSong?.trackId;
    singleton.analyzerData = { analyzer, bufferLength, dataArray, trackid };
  };

  const getTotalTime = () => {
    var time = player.currentSong?.duration
      ? player.currentSong?.duration
      : waveformRef!.current!.duration;
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

  const updateTime = () => {
    if (waveformRef.current && seekerRef.current) {
      var time =
        waveformRef.current?.currentTime && waveformRef.current.currentTime > 0
          ? waveformRef.current.currentTime
          : 0;

      if (waveformRef.current.duration >= 0) {
        seekerRef.current.value = `${
          (waveformRef.current.currentTime / waveformRef.current.duration) *
          10000
        }`;
      } else {
        seekerRef.current.value = "0";
      }

      time = Math.trunc(time);

      var mins = 0;
      var hours = 0;
      if (time > 60) {
        mins = Math.trunc(time / 60);
        hours = Math.trunc(time / (60 * 60));
        if (hours > 0) {
          mins = mins - hours * 60;
        }
      }

      if (mins > 0 && hours == 0) {
        time = time - mins * 60;
      } else if (hours > 0) {
        var hourTime = 60 * 60 * hours;
        var minTime = 60 * mins;
        time = time - (hourTime + minTime);
      } else {
        mins = 0;
      }

      var secondsText = time.toString();
      var minsText = mins.toString();
      var hoursText = hours.toString();

      if (secondsText.length < 2) {
        secondsText = "0" + secondsText;
      }
      if (minsText.length < 2) {
        minsText = "0" + minsText;
      }
      if (hoursText.length < 2) {
        hoursText = "0" + hoursText;
      }
      setTime(`${hoursText}:${minsText}:${secondsText}`);

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

  const updateVolume = (value: string) => {
    var percentage = Number.parseInt(value) / 10000;
    var volume = 1 * percentage;
    setVolume(volume);
    waveformRef!.current!.volume = volume;
  };

  const getPreviousSong = () => {
    if (waveformRef!.current) {
      if (waveformRef.current.currentTime > 3) {
        waveformRef.current.currentTime = 0;
      } else {
        var currentIndex = player.trackList.findIndex(
          (x) => x.artistTrackUploadId == player.currentSong?.trackId
        );

        if (currentIndex > 0) {
          var track = player.trackList[currentIndex - 1];
          dispatch(
            handleSetPlayerSong({
              trackId: track.artistTrackUploadId,
              trackUrl: track.trackUrl,
              currentSongArtistProfile: track.artistProfile,
              trackName: track.trackName,
              artistName: track.artistName,
              trackImage: track.trackImageUrl,
              duration: track.duration,
              uploadDate: track.uploadDate,
            })
          );
        } else {
          waveformRef.current.currentTime = 0;
        }
      }
    }
  };

  const getNextSong = async () => {
    var currentIndex = player.trackList.findIndex(
      (x) => x.artistTrackUploadId == player.currentSong?.trackId
    );

    if (currentIndex + 1 < player.trackList.length) {
      var track = player.trackList[currentIndex + 1];

      dispatch(
        handleSetPlayerSong({
          trackId: track.artistTrackUploadId,
          trackUrl: track.trackUrl,
          currentSongArtistProfile: track.artistProfile,
          trackName: track.trackName,
          artistName: track.artistName,
          trackImage: track.trackImageUrl,
          duration: track.duration,
          uploadDate: track.uploadDate,
        })
      );
    } else {
      if (player.canLoadMore) {
        if (!loadingMore) {
          try {
            setLoadingMore(true);
            var result = await agent.Tracks.getTracks(
              player.listeningProfileMemberId!,
              player.currentSong!.uploadDate,
              player.playlistType!
            );

            if (result.trackList.length > 0) {
              var newTrackList = [...player.trackList, ...result.trackList];
              dispatch(
                handleSetTrackList({
                  trackList: newTrackList,
                  page: player.page + 1,
                  canLoadMore: result.canLoadMore,
                  listeningProfileMemberId: player.listeningProfileMemberId,
                })
              );

              var track = newTrackList[currentIndex + 1];

              dispatch(
                handleSetPlayerSong({
                  trackId: track.artistTrackUploadId,
                  trackUrl: track.trackUrl,
                  currentSongArtistProfile: track.artistProfile,
                  trackName: track.trackName,
                  artistName: track.artistName,
                  trackImage: track.trackImageUrl,
                  duration: track.duration,
                  uploadDate: track.uploadDate,
                })
              );
            } else {
              waveformRef.current!.pause();
              waveformRef.current!.currentTime = 0;
              dispatch(handlePauseSong());
            }
            setLoadingMore(false);
          } catch (err) {
            // do nothing on this error
          }
        }
      } else {
        waveformRef.current!.pause();
        waveformRef.current!.currentTime = 0;
        dispatch(handlePauseSong());
      }
    }
  };

  const goArtistProfile = () => {
    if (player.currentSong?.currentSongArtistProfile) {
      history.push(player.currentSong?.currentSongArtistProfile);
    }
  };

  return (
    <div id="music-player" className="fade-in">
      <Container className="px-3 d-flex flex-column align-items-between h-100">
        <div className="mt-2 player-container mb-2">
          <div className="d-flex flex-row col-12 col-lg-2 track-details">
            <div className="track-image d-flex justify-content-center align-items-center">
              <Image
                size="mini"
                src={
                  player.currentSong?.trackImage
                    ? player.currentSong.trackImage
                    : PlaceholderImg
                }
                className="br-5"
              />
            </div>
            <div
              className="d-flex flex-column ml-2 track-artist"
              onClick={() => goArtistProfile()}
            >
              <div className="track-name">{player.currentSong?.trackName}</div>
              <div className="artist-name">
                {player.currentSong?.artistName}
              </div>
            </div>
          </div>
          <div className="d-flex flex-row col-12 col-lg-10 align-items-center">
            <audio
              id="music"
              preload="all"
              ref={waveformRef}
              onTimeUpdate={() => updateTime()}
            ></audio>
            <div className="pr-3 d-flex flex-row align-items-center">
              {loadingMore ? (
                <InPageLoadingComponent width={40} height={40} />
              ) : (
                <div className="d-flex flex-row fade-in align-items-center">
                  <div className="pr-2">
                    <Icon
                      className="audio-button"
                      name="backward"
                      size="large"
                      color="grey"
                      onClick={() => {
                        getPreviousSong();
                      }}
                    />
                  </div>
                  <div>
                    {!player.currentSong?.playing ? (
                      <Icon
                        className="audio-button m-0 fade-in"
                        name="play"
                        size="large"
                        color="grey"
                        onClick={() => {
                          dispatch(handlePlaySong());
                        }}
                      />
                    ) : (
                      <Icon
                        className="audio-button m-0 fade-in"
                        name="pause"
                        size="large"
                        color="grey"
                        onClick={() => {
                          dispatch(handlePauseSong());
                        }}
                      />
                    )}
                  </div>
                  <div className="pl-2">
                    <Icon
                      className="audio-button"
                      name="forward"
                      size="large"
                      color="grey"
                      onClick={async () => {
                        await getNextSong();
                      }}
                    />
                  </div>
                  <div className="pl-2 volume d-flex align-items-center">
                    {!volumeMuted ? (
                      <Icon
                        name="volume up"
                        size="large"
                        color="grey"
                        className="audio-button"
                        onClick={() => setVolumeMuted(!volumeMuted)}
                      />
                    ) : (
                      <Icon
                        name="volume off"
                        size="large"
                        color="grey"
                        className="audio-button"
                        onClick={() => setVolumeMuted(!volumeMuted)}
                      />
                    )}
                    <div>
                      <div className="volume-control">
                        <input
                          ref={volumeRef}
                          type="range"
                          className="seek-slider volume-slider"
                          max="10000"
                          onChange={(e) => {
                            updateVolume(e.target.value);
                          }}
                        ></input>
                        <div className="inner-arrow"></div>
                        <div className="arrow-down"></div>
                      </div>
                    </div>
                  </div>
                </div>
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
            {time && totalTime ? (
              <h5 className="m-0 pl-3">{totalTime}</h5>
            ) : null}
          </div>
        </div>
      </Container>
    </div>
  );
};

export default MusicPlayer;
