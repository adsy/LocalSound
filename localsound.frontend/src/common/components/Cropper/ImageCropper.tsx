import React, { useState, createRef } from "react";
import Cropper, { ReactCropperElement } from "react-cropper";
import "cropperjs/dist/cropper.css";
import { Button } from "react-bootstrap";
import agent from "../../../api/agent";
import { handleUpdateUserCoverPhoto } from "../../../app/redux/actions/userSlice";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../app/model/redux/state";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";

interface Props {
  file: File;
  setUpdatingCoverPhoto: (updating: boolean) => void;
  setFile: (file: File | null) => void;
}

const ImageCropper = ({ file, setUpdatingCoverPhoto, setFile }: Props) => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const cropperRef = createRef<ReactCropperElement>();
  const dispatch = useDispatch();

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

  const onFileUpload = async (file: File) => {
    // Create an object of formData
    const formData = new FormData();

    if (file) {
      formData.append("fileName", "coverPhoto.jpg");
      formData.append("formFile", file, "coverPhoto.jpg");

      try {
        var result = await agent.Profile.uploadProfileImage(
          userDetail?.memberId!,
          formData,
          AccountImageTypes.CoverImage
        );
        console.log(result);
        console.log("here");
        setFile(null);
        setUpdatingCoverPhoto(false);
        dispatch(handleUpdateUserCoverPhoto(result));
      } catch (err) {
        console.log(err);
      }
    }
  };

  return (
    <div>
      <div style={{ width: "100%" }}>
        <Cropper
          ref={cropperRef}
          style={{ height: 339, width: "100%" }}
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
      </div>
      <Button
        onClick={async () => {
          await getCropData();
        }}
      >
        Accept
      </Button>
    </div>
  );
};

export default ImageCropper;
