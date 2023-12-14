import { createRef, useState } from "react";
import { Button } from "react-bootstrap";
import Cropper, { ReactCropperElement } from "react-cropper";
import { Icon, Image } from "semantic-ui-react";
import { v4 as uuidv4 } from "uuid";

interface Props {
  maxImages?: number;
  images: PhotoUploadModel[];
  setImages: (photos: PhotoUploadModel[]) => void;
}

const MultiImageCropper = ({ maxImages = 3, images, setImages }: Props) => {
  const cropperRef = createRef<ReactCropperElement>();
  const [croppingPhoto, setCroppingPhoto] = useState(false);
  const [photo, setPhoto] = useState<Blob | null>(null);

  const getCropData = async () => {
    if (typeof cropperRef.current?.cropper !== "undefined") {
      var dataURL = cropperRef.current?.cropper.getCroppedCanvas().toDataURL();

      var blobBin = atob(dataURL.split(",")[1]);
      var array = [];
      for (var i = 0; i < blobBin.length; i++) {
        array.push(blobBin.charCodeAt(i));
      }
      var file = new Blob([new Uint8Array(array)], { type: "image/png" });

      var fileUpload = {
        image: file,
        uploadId: uuidv4(),
      };

      setImages([...images, fileUpload]);
      setCroppingPhoto(false);
    }
  };

  const cancelCrop = () => {
    setPhoto(null);
    setCroppingPhoto(false);
  };

  const deletePhoto = (image: PhotoUploadModel) => {
    setImages(images.filter((x) => x.uploadId !== image.uploadId));
  };

  return (
    <div id="multi-cropper">
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
        }}
      >
        {croppingPhoto && photo ? (
          <>
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
              src={URL.createObjectURL(photo)}
              viewMode={3}
              background={false}
              responsive={true}
              autoCropArea={1}
              checkOrientation={false}
              guides={true}
              size={315}
            />

            <div className="crop-action-row d-flex flex-row mt-2 justify-content-center  mb-2">
              <a
                onClick={async () => await getCropData()}
                target="_blank"
                className="btn white-button save-crop-btn"
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
          </>
        ) : images.length < maxImages ? (
          <div className="d-flex flex-column justify-content-center align-items-center">
            <div className="multi-image-placeholder"></div>
            <label
              htmlFor="packageUpload"
              className="btn mt-2 mb-2 white-button fade-in-out w-fit-content align-self-center px-5"
            >
              <h4>Select photo</h4>
            </label>
            <input
              type="file"
              accept=".jpg,.png,.jpeg"
              id="packageUpload"
              style={{ display: "none" }}
              onChange={async (event) => {
                if (event?.target?.files) {
                  setPhoto(event.target.files[0]);
                  setCroppingPhoto(true);
                }
              }}
            />
          </div>
        ) : null}
        {images.length > 0 ? (
          <div className="d-flex flex-row justify-content-center">
            {images.map((image, index) => (
              <div key={index} className="position-relative">
                <Button
                  className="black-button bin-icon"
                  onClick={() => deletePhoto(image)}
                >
                  <Icon
                    name="trash"
                    size="small"
                    className="m-0 d-flex flex-row justify-content-center"
                  />
                </Button>
                <Image
                  size="tiny"
                  src={URL.createObjectURL(image.image)}
                  className="mr-2 image-display"
                />
              </div>
            ))}
          </div>
        ) : null}
      </div>
    </div>
  );
};

export default MultiImageCropper;
