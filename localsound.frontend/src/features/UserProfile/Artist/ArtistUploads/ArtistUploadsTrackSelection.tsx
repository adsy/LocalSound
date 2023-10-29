import { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import { Icon } from "semantic-ui-react";

interface Props {
  setFile: (file: File | null) => void;
  setTrackExt: (ext: string) => void;
}

const ArtistUploadsTrackSelection = ({ setFile, setTrackExt }: Props) => {
  const onDrop = useCallback(async (acceptedFile: File[]) => {
    if (acceptedFile[0]) {
      var trackExt = acceptedFile[0].name.split(/[.]+/).pop();
      setTrackExt(trackExt!);
      setFile(acceptedFile[0]);
    }
  }, []);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    multiple: false,
  });

  return (
    <div>
      <p className="mb-2">
        Showcase your skills! Get started by clicking the zone below or dragging
        your track into it.
      </p>

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
