import { useState } from "react";
import { Image } from "semantic-ui-react";
import agent from "../../../../../api/agent";
import { useDispatch, useSelector } from "react-redux";
import { State } from "../../../../../app/model/redux/state";
import img from "../../../../../assets/icons/user.svg";
import { AccountImageTypes } from "../../../../../app/model/enums/accountImageTypes";
import { handleUpdateUserCoverPhoto } from "../../../../../app/redux/actions/userSlice";
import { handleResetModal } from "../../../../../app/redux/actions/modalSlice";

const EditArtistPhotosForm = () => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const [file, setFile] = useState<File | null>(null);
  const dispatch = useDispatch();

  const onFileUpload = async () => {
    // Create an object of formData
    const formData = new FormData();

    if (file) {
      // Update the formData object
      formData.append("formFile", file);
      formData.append("fileName", file.name);

      try {
        var result = await agent.Profile.uploadProfileImage(
          userDetail?.memberId!,
          formData,
          AccountImageTypes.CoverImage
        );

        dispatch(handleResetModal());
        dispatch(handleUpdateUserCoverPhoto(result));
      } catch (err) {
        console.log(err);
      }
    }
  };

  return (
    <div>
      <h3 className="mt-5">Cover photo</h3>
      <Image src={img} size="small" circular className="mb-2 profile-img" />
      <input
        type="file"
        onChange={(event) => {
          if (event && event.target && event.target.files) {
            setFile(event.target.files[0]);
          }
        }}
      />
      <button onClick={async () => await onFileUpload()}>Upload!</button>
    </div>
  );
};

export default EditArtistPhotosForm;
