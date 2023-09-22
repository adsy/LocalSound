import { useLayoutEffect, useRef, useState } from "react";
import { ArtistTrackUploadModel } from "../../../app/model/dto/artist-track-upload.model";
import WaveFormContainer from "./WaveFormContainer";
import PlayButton from "./PlayButton";
import WaveSurfer from "wavesurfer.js";

interface Props {
  track: ArtistTrackUploadModel;
}

const WaveForm = ({ track }: Props) => {
  const [playing, setPlaying] = useState(false);
  const waveformRef = useRef<HTMLDivElement>(null);
  const [waveForm, setWaveForm] = useState<WaveSurfer | null>(null);

  useLayoutEffect(() => {
    let waveform = WaveSurfer.create({
      barWidth: 3,
      cursorWidth: 1,
      container: waveformRef.current!,
      //   backend: "WebAudio",
      height: 80,
      progressColor: "#2D5BFF",
      //   responsive: true,
      waveColor: "#EFEFEF",
      cursorColor: "transparent",
    });

    waveform.load(
      "https://localsoundstorage.blob.core.windows.net/d872fc6d-47df-4ef3-3665-08db56bd8a55/uploads/Hermitude - Tapedeck Sound.mp3"
    );

    setWaveForm(waveform);
  }, []);

  const handlePlay = () => {
    setPlaying(!playing);
    waveForm!.playPause();
  };

  return (
    <div id="waveform">
      <WaveFormContainer>
        <PlayButton handlePlay={handlePlay} playing={playing} />
        <div id="waveform" ref={waveformRef} />
      </WaveFormContainer>
    </div>
  );
};

export default WaveForm;
