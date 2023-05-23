import { useSelector } from "react-redux";
import { State } from "../../app/model/redux/state";
import bg from "../../assets/landing-page-banner/banner2.jpg";
import img from "../../assets/icons/user.svg";
import { Row } from "react-bootstrap";
import { Image } from "semantic-ui-react";

const ArtistProfile = () => {
  const userDetails = useSelector((state: State) => state.user.userDetails);

  const bannerStyle = {
    backgroundImage: `url(${bg})`,
    backgroundSize: "cover",
    backgroundAttachment: "inherit",
    backgroundPosition: "center center",
    backgroundRepeat: "no-repeat",
    height: "25rem",
  };

  return (
    <>
      <Row>
        <div style={bannerStyle} className="profile-banner position-relative">
          <div className="details-container">
            <Image src={img} size="tiny" circular className="profile-img" />
            <h1 className="user-name align-self-center ml-1">
              {userDetails?.name}
            </h1>
          </div>
        </div>
      </Row>
      <Row className="profile-2-row justify-content-between">
        <div className="col-12 col-sm-6">
          <div>Followers</div>
        </div>
        <div className="col-12 col-sm-6">
          <div>Upcoming gigs</div>
        </div>
      </Row>
      <Row>
        <div>Uploaded tracks</div>
      </Row>
    </>
  );
};

export default ArtistProfile;
