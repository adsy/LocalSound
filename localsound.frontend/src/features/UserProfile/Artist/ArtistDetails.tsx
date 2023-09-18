import { Col } from "react-bootstrap";
import ErrorBanner from "../../../common/banner/ErrorBanner";
import { UserModel } from "../../../app/model/dto/user.model";
import Label from "../../../common/components/Label/Label";

interface Props {
  photoUpdateError: string | null;
  userDetails: UserModel;
}

const ArtistDetails = ({ photoUpdateError, userDetails }: Props) => {
  return (
    <div className="d-flex flex-row flex-wrap">
      <Col md={12} lg={4} className="p-0 left-col">
        <div className="d-flex flex-column p-2 py-0">
          {photoUpdateError ? (
            <div className="d-flex flex-row">
              <ErrorBanner className="w-100">{photoUpdateError}</ErrorBanner>
            </div>
          ) : null}
          <div className="d-flex flex-column pb-4">
            <div className="d-flex">
              <h4 className="section-title inverse">Followers</h4>
            </div>
            <div className="d-flex flex-row">
              <span className="about-text m-0 pr-3">0 followers</span>
              <span className="about-text m-0">0 following</span>
            </div>
          </div>
          <div className="d-flex flex-column pb-2 genre-mobile">
            <div className="d-flex">
              <h4 className="section-title inverse">Genres</h4>
            </div>
            <div className="about-text">
              {userDetails.genres.map((genre, index) => (
                <span key={index}>
                  <Label label={genre.genreName} id={genre.genreId} />
                </span>
              ))}
            </div>
          </div>
          <div className="d-flex flex-column pb-4">
            <div className="d-flex">
              <h4 className="section-title inverse">Event types</h4>
            </div>
            <div className="about-text">
              {userDetails.eventTypes.map((eventType, index) => (
                <span key={index}>
                  <Label
                    label={eventType.eventTypeName}
                    id={eventType.eventTypeId}
                  />
                </span>
              ))}
            </div>
          </div>
          <div className="d-flex flex-column pb-4">
            <div className="d-flex">
              <h4 className="section-title inverse">Equipment</h4>
            </div>
            <div className="about-text">
              {userDetails.equipment.map((equipment, index) => (
                <span key={index}>
                  <Label
                    label={equipment.equipmentName}
                    id={equipment.equipmentId}
                  />
                </span>
              ))}
            </div>
          </div>
        </div>
      </Col>
      <Col md={12} lg={8} className="p-0 right-col">
        <div className="p-2 py-0">
          <div className="d-flex flex-column pb-4">
            <div className="d-flex">
              <h4 className="section-title inverse">About</h4>
            </div>
            <span className="about-text">{userDetails.aboutSection}</span>
          </div>
        </div>
      </Col>
    </div>
  );
};

export default ArtistDetails;
