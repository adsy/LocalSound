import logo from "../../assets/updated-logo4.png";

const LandingPageBanner = () => {
  return (
    <div className="banner mb-4 d-flex flex-column justify-content-center">
      <div className="d-flex flex-column justify-content-between w-100 mt-5">
        <div className="d-flex flex-row justify-content-center align-content-center pr-2 pt-1 pb-1">
          <img
            alt=""
            src={logo}
            width="50"
            height="50"
            className="d-inline-block align-top"
          />
          <h1 className="m-0 pl-2 align-self-center banner-title">
            LocalSound
          </h1>
        </div>
        <div
          id="landing-page-search"
          className="d-flex flex-column justify-content-start"
        >
          <h4 className="text-center search-text">
            Dont rely on your friends "heaters🔥🔥" playlist.
          </h4>
        </div>
      </div>
    </div>
  );
};
export default LandingPageBanner;
