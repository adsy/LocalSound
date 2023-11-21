import { createRef } from "react";
import Cropper, { ReactCropperElement } from "react-cropper";
import "cropperjs/dist/cropper.css";
import { CropTypes } from "../../../app/model/enums/cropTypes";
import InPageLoadingComponent from "../../../app/layout/InPageLoadingComponent";

interface Props {
  file: File;
  onFileUpload: (file: Blob) => void;
  cancelCrop: () => void;
  cropType: CropTypes;
  submittingPhoto?: boolean;
}

const ImageCropper = ({
  file,
  onFileUpload,
  cancelCrop,
  cropType,
  submittingPhoto,
}: Props) => {
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
        style={
          cropType === CropTypes.Circle
            ? {
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
              }
            : cropType === CropTypes.Flexible
            ? { width: "100%", position: "relative" }
            : {
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
              }
        }
      >
        {cropType === CropTypes.Flexible ? (
          <Cropper
            ref={cropperRef}
            style={{ height: "30rem", width: "100%", opacity: ".7" }}
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
        ) : cropType === CropTypes.Circle ? (
          <Cropper
            ref={cropperRef}
            style={{
              height: "315px",
              width: "315px",
              alignSelf: "center",
            }}
            zoomTo={0.5}
            aspectRatio={1 / 1}
            preview=".img-preview"
            src={URL.createObjectURL(file)}
            viewMode={3}
            minCropBoxHeight={315}
            minCropBoxWidth={315}
            background={false}
            responsive={false}
            autoCropArea={0.5}
            checkOrientation={false}
            guides={true}
            scalable={false}
          />
        ) : (
          <Cropper
            ref={cropperRef}
            style={{
              height: "315px",
              width: "315px",
              alignSelf: "center",
            }}
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
            size={315}
          />
        )}

        {!submittingPhoto ? (
          <div className="crop-action-row d-flex flex-row mt-2 justify-content-center">
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
              className="ml-1 btn white-button save-crop-btn"
            >
              <h4>Cancel</h4>
            </a>
          </div>
        ) : (
          <InPageLoadingComponent />
        )}
      </div>
    </div>
  );
};

export default ImageCropper;
