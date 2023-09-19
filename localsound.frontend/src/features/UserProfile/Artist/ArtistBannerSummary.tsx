import { Image } from "semantic-ui-react";
import { UserModel } from "../../../app/model/dto/user.model";
import Label from "../../../common/components/Label/Label";
import { AccountImageTypes } from "../../../app/model/enums/accountImageTypes";
import userImg from "../../../assets/icons/user.svg";

interface Props {
  userDetails: UserModel;
}

const ArtistBannerSummary = ({ userDetails }: Props) => {
  const userPhoto = userDetails.images.find(
    (x) => x.accountImageTypeId == AccountImageTypes.ProfileImage
  );
  return (
    <div className="d-flex flex-column">
      <div className="d-flex flex-row ml-2">
        <div className="d-flex flex-column">
          <div className="d-flex flex-row flex-wrap mb-1">
            <div className="d-flex flex-column justify-content-end">
              <Image
                src={userPhoto ? userPhoto.accountImageUrl : userImg}
                size="small"
                circular
                className={`${userPhoto ? "profile-photo" : null} banner-photo`}
              />
              <span className="user-name mb-0">{userDetails?.name}</span>
              <div className=" d-flex flex-row flex-wrap">
                {userDetails?.soundcloudUrl ? (
                  <>
                    <a
                      href={userDetails.soundcloudUrl}
                      target="_blank"
                      className="btn soundcloud-button w-fit-content d-flex flex-row mb-1 mr-1"
                    >
                      <div className="soundcloud-icon align-self-center mr-2"></div>
                      <h4 className="align-self-center mt-0">Soundcloud</h4>
                    </a>
                  </>
                ) : null}
                {userDetails?.spotifyUrl ? (
                  <>
                    <a
                      href={userDetails.spotifyUrl}
                      target="_blank"
                      className="btn spotify-button w-fit-content d-flex flex-row mb-1 mr-1"
                    >
                      <div className="spotify-icon align-self-center mr-2"></div>
                      <h4 className="align-self-center mt-0">Spotify</h4>
                    </a>
                  </>
                ) : null}
                {userDetails?.youtubeUrl ? (
                  <>
                    <a
                      href={userDetails.youtubeUrl}
                      target="_blank"
                      className="btn youtube-button w-fit-content d-flex flex-row mb-3"
                    >
                      <div className="soundcloud-icon align-self-center mr-2"></div>
                      <h4 className="align-self-center mt-0">Youtube</h4>
                    </a>
                  </>
                ) : null}
              </div>
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
    </div>
  );
};

export default ArtistBannerSummary;
