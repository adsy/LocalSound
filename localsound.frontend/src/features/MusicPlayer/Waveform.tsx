// WaveForm.jsx

import { useRef, useEffect } from "react";
import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";

interface Props {
  analyzerData: any;
  trackId: string;
  playing: boolean;
}

// Component to render the waveform
const WaveForm = ({ analyzerData, trackId, playing }: Props) => {
  // Ref for the canvas element
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const player = useSelector((state: State) => state.player);

  const { dataArray, analyzer, bufferLength, trackid } = analyzerData;

  const animateBars = (
    analyser: any,
    canvas: any,
    canvasCtx: any,
    dataArray: any,
    bufferLength: any
  ) => {
    console.log(trackId);
    // Analyze the audio data using the Web Audio API's `getByteFrequencyData` method.
    analyser.getByteFrequencyData(dataArray);

    // Set the canvas fill style to black.
    canvasCtx.fillStyle = "#000";

    // Calculate the height of the canvas.
    const HEIGHT = canvas.height / 1;

    // Calculate the width of each bar in the waveform based on the canvas width and the buffer length.
    var barWidth = Math.ceil(canvas.width / bufferLength) * 2.5;

    // Initialize variables for the bar height and x-position.
    let barHeight;
    let x = 0;

    // Loop through each element in the `dataArray`.
    for (var i = 0; i < bufferLength; i++) {
      // Calculate the height of the current bar based on the audio data and the canvas height.
      barHeight = (dataArray[i] / 255) * HEIGHT;

      // Generate random RGB values for each bar.
      const maximum = 20;
      const minimum = -20;
      var r = 0 + Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;
      var g = 0 + Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;
      var b = 0 + Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;

      // Set the canvas fill style to the random RGB values..
      canvasCtx.fillStyle = "rgb(" + r + "," + g + "," + b + ")";

      // Draw the bar on the canvas at the current x-position and with the calculated height and width.
      canvasCtx.fillRect(x, HEIGHT - barHeight, barWidth, barHeight);

      // Update the x-position for the next bar.
      x += barWidth + 1;
    }
  };

  // Function to draw the waveform
  const draw = (dataArray: any, analyzer: any, bufferLength: any) => {
    if (canvasRef.current) {
      const canvas = canvasRef.current;
      if (!canvas || !analyzer) return;
      const canvasCtx = canvas.getContext("2d");

      const animate = () => {
        requestAnimationFrame(animate);
        canvas.width = canvas.width;
        canvas.height = 100;

        var id = trackid;
        if (player.playing) {
          animateBars(analyzer, canvas, canvasCtx, dataArray, bufferLength);
        }
      };

      animate();
    }
  };

  useEffect(() => {
    console.log(playing);
  }, [playing]);

  // Effect to draw the waveform on mount and update
  useEffect(() => {
    if (playing) {
      draw(dataArray, analyzer, bufferLength);
    } else {
      console.log("here");
    }
  }, [dataArray, analyzer, bufferLength, playing]);

  // Return the canvas element
  return (
    <canvas
      style={{
        position: "absolute",
        top: "0",
        left: "0",
        zIndex: "1",
        width: "100%",
        height: "100%",
      }}
      ref={canvasRef}
      width={window.innerWidth}
      height={window.innerHeight}
    />
  );
};

export default WaveForm;
