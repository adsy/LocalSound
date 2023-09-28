import React, { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import { Icon } from "semantic-ui-react";

interface Props {
  setFile: (file: File | null) => void;
  setTrackExt: (ext: string) => void;
  setDps: (dps: any) => void;
}

const ArtistUploadsTrackSelection = ({
  setFile,
  setTrackExt,
  setDps,
}: Props) => {
  const onDrop = useCallback(async (acceptedFile: File[]) => {
    if (acceptedFile[0]) {
      var trackExt = acceptedFile[0].name.split(/[.]+/).pop();
      setTrackExt(trackExt!);
      // await generateWaveForm(acceptedFile[0]);
      setFile(acceptedFile[0]);
    }
  }, []);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    multiple: false,
  });

  // const generateWaveForm = async (file: File) => {
  //   let margin = 0,
  //     chunkSize = 100000,
  //     height = 1000,
  //     scaleFactor = (height - margin * 2) / 1;

  //   var audioContext = new AudioContext();

  //   let buffer = await file!.arrayBuffer(),
  //     audioBuffer = await audioContext.decodeAudioData(buffer),
  //     float32Array = audioBuffer.getChannelData(0);

  //   let array = [],
  //     i = 0,
  //     length = float32Array.length;

  //   while (i < length) {
  //     array.push(
  //       float32Array.slice(i, (i += chunkSize)).reduce(function (total, value) {
  //         return Math.max(total, Math.abs(value));
  //       })
  //     );
  //   }
  //   let dps = [];
  //   for (let index in array) {
  //     dps.push({
  //       x: margin + Number(index),
  //       y: 50 + array[index] * scaleFactor,
  //     });
  //   }

  //   setDps(dps);
  // };

  return (
    <div id="">
      <h5 className="inverse">
        Showcase your skills! Get started by clicking the zone below or dragging
        your track into it.
      </h5>

      <div {...getRootProps()}>
        <input {...getInputProps()} />
        {isDragActive ? (
          <div className="d-flex flex-row justify-content-center dropzone">
            <div className="d-flex flex-column justify-content-center align-self-center justify-content-center">
              <Icon name="upload" size={"huge"} className="align-self-center" />
              <p className="inverse align-self-center mt-4">
                Drop the files here ...
              </p>
            </div>
          </div>
        ) : (
          <div className="d-flex flex-row justify-content-center dropzone">
            <div className="d-flex flex-column justify-content-center align-self-center justify-content-center">
              <Icon name="upload" size={"huge"} className="align-self-center" />
              <p className="inverse align-self-center mt-4">
                Drag your file in here or click to select your file.
              </p>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ArtistUploadsTrackSelection;
