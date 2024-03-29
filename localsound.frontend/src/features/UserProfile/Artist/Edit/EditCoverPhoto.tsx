import ImageCropper from "../../../../common/components/Cropper/ImageCropper";
import agent from "../../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../../app/model/redux/state";
import { AccountImageTypes } from "../../../../app/model/enums/accountImageTypes";
import { handleUpdateUserCoverPhoto } from "../../../../app/redux/actions/userSlice";
import { CropTypes } from "../../../../app/model/enums/cropTypes";
import { handleAppLoading } from "../../../../app/redux/actions/applicationSlice";
import { handleUpdateProfileCoverPhoto } from "../../../../app/redux/actions/pageDataSlice";

interface Props {
  file: File;
  setUpdatingCoverPhoto: (updating: boolean) => void;
  setFile: (file: File | null) => void;
  setPhotoUpdateError: (photoUpdateError: string) => void;
}

const EditCoverPhoto = ({
  file,
  setUpdatingCoverPhoto,
  setFile,
  setPhotoUpdateError,
}: Props) => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const dispatch = useDispatch();

  const onFileUpload = async (file: Blob) => {
    const formData = new FormData();

    if (file) {
      formData.append("fileName", file.name);
      formData.append("formFile", file);
      formData.append("fileExt", `.png`);

      try {
        dispatch(handleAppLoading(true));
        var result = await agent.Account.uploadProfileImage(
          userDetail?.memberId!,
          formData,
          AccountImageTypes.CoverImage
        );

        setFile(null);
        dispatch(handleUpdateUserCoverPhoto(result));
        dispatch(handleUpdateProfileCoverPhoto(result));
        setUpdatingCoverPhoto(false);
      } catch (err) {
        setPhotoUpdateError(
          "There was an error updating your cover photo, please try again.."
        );
      }
      dispatch(handleAppLoading(false));
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
        cropType={CropTypes.Flexible}
      />
    </>
  );
};

export default EditCoverPhoto;
