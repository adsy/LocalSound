import { createRef } from "react";
import { Cropper, ReactCropperElement } from "react-cropper";

interface Props {
  file: File;
  onFileUpload: (blob: Blob) => void;
  cancelCrop: () => void;
}

const CircleCropper = ({ file, onFileUpload, cancelCrop }: Props) => {
  const cropperRef = createRef<ReactCropperElement>();

  const getCropData = async () => {
    if (typeof cropperRef.current?.cropper !== "undefined") {
      var dataURL = cropperRef.current?.cropper.getCroppedCanvas().toDataURL();

      var blobBin = atob(dataURL.split(",")[1]);
      var array = [];
      for (var i = 0; i < blobBin.length; i++) {
        array.push(blobBin.charCodeAt(i));
      }
      var file = new Blob([new Uint8Array(array)], { type: "image/png" });

      await onFileUpload(file);
    }
  };

  return (
    <div id="cropper">
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
        }}
      >
        <Cropper
          ref={cropperRef}
          style={{ height: "315px", width: "315px", alignSelf: "center" }}
          zoomTo={0.5}
          aspectRatio={1 / 1}
          preview=".img-preview"
          src={URL.createObjectURL(file)}
          viewMode={3}
          background={false}
          responsive={true}
          autoCropArea={1}
          checkOrientation={false}
          guides={true}
          size={300}
        />
        <div className="crop-action-row d-flex flex-row justify-content-center mt-2">
          <a
            onClick={async () => await getCropData()}
            target="_blank"
            className="btn black-button save-crop-btn"
          >
            <h4>Upload</h4>
          </a>
          <a
            onClick={() => cancelCrop()}
            target="_blank"
            className="ml-1 btn purple-button save-crop-btn"
          >
            <h4>Cancel</h4>
          </a>
        </div>
      </div>
    </div>
  );
};

export default CircleCropper;
