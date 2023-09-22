import React, { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import { Icon } from "semantic-ui-react";

interface Props {
  setFile: (file: File | null) => void;
}

const ArtistUploadsTrackSelection = ({ setFile }: Props) => {
  const onDrop = useCallback((acceptedFile: File[]) => {
    if (acceptedFile[0]) {
      setFile(acceptedFile[0]);
    }
  }, []);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    multiple: false,
  });

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
