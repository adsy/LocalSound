import { Image } from "semantic-ui-react";
import { UserModel } from "../../../app/model/dto/user.model";
import Label from "../../../common/components/Label/Label";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import userImg from "../../../assets/icons/user.svg";

interface Props {
  userDetails: UserModel;
  addLeftSpacing?: boolean;
}

const ArtistSummary = ({ userDetails, addLeftSpacing = true }: Props) => {
  const userPhoto = userDetails.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
  );
  return (
    <div className={`d-flex flex-row ${addLeftSpacing ? "ml-2" : ""}`}>
      <div className="d-flex flex-column">
        <div className="d-flex flex-row flex-wrap mb-1">
          <div className="d-flex flex-column justify-content-end">
            <span className="user-name mb-0">{userDetails?.name}</span>
          </div>
        </div>

        <div className="d-flex flex-column pb-2 genre-desktop">
          <div className="about-text">
            {userDetails.genres.map((genre, index) => (
              <span key={index}>
                <Label label={genre.genreName} id={genre.genreId} />
              </span>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ArtistSummary;
