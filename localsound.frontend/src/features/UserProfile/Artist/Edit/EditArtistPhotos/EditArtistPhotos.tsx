import { UserModel } from "../../../../../app/model/dto/user.model";
import EditArtistPhotosForm from "./EditArtistPhotosForm";

interface Props {
  userDetails: UserModel;
}

const EditArtistPhotos = ({ userDetails }: Props) => {
  return (
    <>
      <EditArtistPhotosForm />
    </>
  );
};

export default EditArtistPhotos;
