import React, { useState, createRef } from "react";
import Cropper, { ReactCropperElement } from "react-cropper";
import "cropperjs/dist/cropper.css";

interface Props {
  file: File;
  onFileUpload: (file: Blob) => void;
  cancelCrop: () => void;
}

const ImageCropper = ({ file, onFileUpload, cancelCrop }: Props) => {
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
      <div style={{ width: "100%", position: "relative" }}>
        <Cropper
          ref={cropperRef}
          style={{ height: "24rem", width: "100%", opacity: ".7" }}
          zoomTo={0.5}
          initialAspectRatio={659 / 336}
          preview=".img-preview"
          src={URL.createObjectURL(file)}
          viewMode={1}
          minCropBoxHeight={10}
          minCropBoxWidth={10}
          background={false}
          responsive={true}
          autoCropArea={1}
          checkOrientation={false}
          guides={true}
        />
        <div className="crop-action-row d-flex flex-row">
          <a
            onClick={async () => await getCropData()}
            target="_blank"
            className="btn black-button save-crop-btn"
          >
            <h4>Save</h4>
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
      {/* <Button>Cancel</Button> */}
    </div>
  );
};

export default ImageCropper;
