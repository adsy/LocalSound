import { UserModel } from "../../app/model/dto/user.model";
import Label from "../../common/components/Label/Label";

interface Props {
  userDetails: UserModel;
  addLeftSpacing?: boolean;
  isArtist: boolean;
}

const ProfileSummary = ({
  userDetails,
  isArtist,
  addLeftSpacing = true,
}: Props) => {
  return (
    <div className={`d-flex flex-row ${addLeftSpacing ? "ml-2" : ""}`}>
      <div className="d-flex flex-column">
        <div className="d-flex flex-row flex-wrap mb-1">
          <div className="d-flex flex-column justify-content-end">
            {isArtist ? (
              <span className="user-name mb-0">{userDetails?.name}</span>
            ) : (
              <span className="user-name mb-0">
                {userDetails?.firstName} {userDetails?.lastName}
              </span>
            )}
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

export default ProfileSummary;
