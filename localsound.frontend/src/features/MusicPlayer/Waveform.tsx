// WaveForm.jsx

import { useRef, useEffect } from "react";
import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";

interface Props {
  analyzerData: any;
}

// Component to render the waveform
const WaveForm = ({ analyzerData }: Props) => {
  // Ref for the canvas element
  const canvasRef = useRef<HTMLCanvasElement>(null);
  const player = useSelector((state: State) => state.player);

  const { dataArray, analyzer, bufferLength } = analyzerData;

  const animateBars = (
    analyser: any,
    canvas: any,
    canvasCtx: any,
    dataArray: any,
    bufferLength: any
  ) => {
    // Analyze the audio data using the Web Audio API's `getByteFrequencyData` method.
    analyser.getByteFrequencyData(dataArray);

    // Set the canvas fill style to black.
    canvasCtx.fillStyle = "#000";

    // Calculate the height of the canvas.
    const HEIGHT = canvas.height / 1;

    // Calculate the width of each bar in the waveform based on the canvas width and the buffer length.
    var barWidth = Math.ceil(canvas.width / bufferLength) * 1.5;

    // Initialize variables for the bar height and x-position.
    let barHeight;
    let x = 0;

    // Loop through each element in the `dataArray`.
    for (var i = 0; i < bufferLength; i++) {
      // Calculate the height of the current bar based on the audio data and the canvas height.
      barHeight = (dataArray[i] / 255) * HEIGHT;

      // Generate random RGB values for each bar.
      // const maximum = 20;
      // const minimum = -20;
      // var r = 0 + Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;
      // var g = 0 + Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;
      // var b = 0 + Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;

      // Set the canvas fill style to the random RGB values..
      // canvasCtx.fillStyle = "rgb(109,105,250)";
      canvasCtx.fillStyle = "rgb(0,0,0)";

      // Draw the bar on the canvas at the current x-position and with the calculated height and width.
      canvasCtx.fillRect(x, HEIGHT - barHeight, barWidth, barHeight);

      // Update the x-position for the next bar.
      x += barWidth + 1;
    }
  };

  const animate = () => {
    if (canvasRef.current) {
      const canvas = canvasRef.current;
      if (!canvas || !analyzer) return;

      requestAnimationFrame(animate);
      canvas.width = canvas.offsetWidth;
      canvas.height = canvas.offsetHeight;
      const canvasCtx = canvas.getContext("2d");

      if (player.currentSong?.playing) {
        animateBars(analyzer, canvas, canvasCtx, dataArray, bufferLength);
      }
    }
  };

  // Effect to draw the waveform on mount and update
  useEffect(() => {
    animate();

    return () => {
      if (canvasRef.current) canvasRef.current.remove();
    };
  }, [dataArray, analyzer, bufferLength]);

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
