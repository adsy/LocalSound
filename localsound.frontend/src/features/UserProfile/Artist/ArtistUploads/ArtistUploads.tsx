import { UserModel } from "../../../../app/model/dto/user.model";
import ArtistUploadsList from "./ArtistUploadsList";

interface Props {
  userDetails: UserModel;
}

const ArtistUploads = ({ userDetails }: Props) => {
  return (
    <>
      <ArtistUploadsList userDetails={userDetails} />
    </>
  );
};

export default ArtistUploads;
