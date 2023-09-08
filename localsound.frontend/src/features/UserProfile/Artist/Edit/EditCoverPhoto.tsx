import ImageCropper from "../../../../common/components/Cropper/ImageCropper";
import agent from "../../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../../app/model/redux/state";
import { AccountImageTypes } from "../../../../app/model/enums/accountImageTypes";
import { handleUpdateUserCoverPhoto } from "../../../../app/redux/actions/userSlice";

interface Props {
  file: File;
  setUpdatingCoverPhoto: (updating: boolean) => void;
  setFile: (file: File | null) => void;
  setSubmittingRequest: (submittingRequest: boolean) => void;
}

const EditCoverPhoto = ({
  file,
  setUpdatingCoverPhoto,
  setFile,
  setSubmittingRequest,
}: Props) => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const onFileUpload = async (file: Blob) => {
    const formData = new FormData();

    if (file) {
      formData.append("fileName", "coverPhoto.jpg");
      formData.append("formFile", file, "coverPhoto.jpg");

      try {
        setSubmittingRequest(true);
        var result = await agent.Profile.uploadProfileImage(
          userDetail?.memberId!,
          formData,
          AccountImageTypes.CoverImage
        );

        setFile(null);
        setUpdatingCoverPhoto(false);
        dispatch(handleUpdateUserCoverPhoto(result));
        setSubmittingRequest(false);
      } catch (err) {
        //TODO: Do something with the errors here
      }
    }
  };

  const cancelCrop = () => {
    setFile(null);
    setUpdatingCoverPhoto(false);
  };

  return (
    <>
      <ImageCropper
        file={file!}
        onFileUpload={onFileUpload}
        cancelCrop={cancelCrop}
      />
    </>
  );
};

export default EditCoverPhoto;