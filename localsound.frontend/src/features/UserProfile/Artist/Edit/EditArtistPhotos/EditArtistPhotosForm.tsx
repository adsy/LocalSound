import { useState } from "react";
import { Divider } from "semantic-ui-react";
import agent from "../../../../../api/agent";
import { useSelector } from "react-redux";
import { State } from "../../../../../app/model/redux/state";

const EditArtistPhotosForm = () => {
  const userDetail = useSelector((state: State) => state.user.userDetails);
  const [file, setFile] = useState<File | null>(null);

  const onFileUpload = async () => {
    // Create an object of formData
    const formData = new FormData();

    if (file) {
      // Update the formData object
      formData.append("formFile", file);
      formData.append("fileName", file.name);
      console.log(formData);
      try {
        await agent.Profile.uploadProfileImage(userDetail?.memberId!, formData);
      } catch (err) {
        console.log(err);
      }
    }
  };

  return (
    <div>
      <h3 className="mt-5">Profile image</h3>
      <input
        type="file"
        onChange={(event) => {
          if (event && event.target && event.target.files) {
            setFile(event.target.files[0]);
          }
        }}
      />
      <button onClick={async () => await onFileUpload()}>Upload!</button>
      <Divider />
      <h3 className="mt-2">Cover photo</h3>
    </div>
  );
};

export default EditArtistPhotosForm;
